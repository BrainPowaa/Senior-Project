using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using VoxDOTS;

/**
 * Chunk mesh renderer.
 * 
 * Voxel data is passed in to a Compute Buffer and the mesh is
 * generated entirely on the GPU for high performance
 */
[ExecuteInEditMode]
public class ComputeMeshTest : MonoBehaviour
{
    public Material drawMeshMaterial;
    public ComputeShader chunkMeshComputeShader;
    public ComputeShader chunkDataComputeShader;

    public bool update = true;

    public Vector3Int chunkCount = new Vector3Int(1, 1, 1);

    private ComputeBuffer _chunkDataBuffer, _visibleVoxelBuffer, _meshBuffer, _drawArgsBuffer, _tempOffsetBuffer;

    private int[] visibleVoxelArray = new []{0};

    [StructLayout(LayoutKind.Sequential)]
    struct DrawArg
    {
        public int vertexCountPerInstance;
        public int instanceCount;
        public int startVertexLocation;
        public int startInstanceLocation;
    }

    void OnEnable()
    {
        // TEMP: Voxel count, use actual constants later.
        long maxVoxelCount = Constants.MaxChunkSize * chunkCount.x * chunkCount.y * chunkCount.z;
        maxVoxelCount = Math.Min(maxVoxelCount, 62914560/12);
        
        Debug.Log("VoxCnt " + maxVoxelCount * 12);
        
        chunkDataComputeShader.SetInt("ChunkX", Constants.ChunkSize * chunkCount.x);
        chunkDataComputeShader.SetInt("ChunkY", Constants.ChunkSize * chunkCount.y);
        chunkDataComputeShader.SetInt("ChunkZ", Constants.ChunkSize * chunkCount.z);
        
        drawMeshMaterial.SetInt("ChunkX", Constants.ChunkSize * chunkCount.x);
        drawMeshMaterial.SetInt("ChunkY", Constants.ChunkSize * chunkCount.y);
        drawMeshMaterial.SetInt("ChunkZ", Constants.ChunkSize * chunkCount.z);

        // Voxel data buffer on the GPU.
        _chunkDataBuffer = new ComputeBuffer((int)maxVoxelCount, sizeof(int));

        _visibleVoxelBuffer = new ComputeBuffer(1, sizeof(int));
        _visibleVoxelBuffer.SetData(visibleVoxelArray);

        // Create buffer that has draw args that will be assigned on the GPU.
        _drawArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);
        _drawArgsBuffer.SetData(new[] {new DrawArg {vertexCountPerInstance = 0, instanceCount = 1}});

        _tempOffsetBuffer = new ComputeBuffer(1, sizeof(float));

        UpdateBuffers();
    }

    private void OnDisable()
    {
        _chunkDataBuffer.Release();
        _visibleVoxelBuffer.Release();
        _drawArgsBuffer.Release();
        _meshBuffer.Release();
        _tempOffsetBuffer.Release();
    }

    void UpdateBuffers()
    {
        var threadsX = (Constants.ChunkSize / 8) * chunkCount.x;
        var threadsY = (Constants.ChunkSize / 8) * chunkCount.y;
        var threadsZ = (Constants.ChunkSize / 8) * chunkCount.z;
        
        _visibleVoxelBuffer.SetData(new[] {0});

        // Assign buffers.
        chunkDataComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        chunkDataComputeShader.SetBuffer(0, "VisibleVoxelCount", _visibleVoxelBuffer);
        chunkDataComputeShader.SetBuffer(0, "Offset", _tempOffsetBuffer);
        chunkDataComputeShader.SetBuffer(1, "Offset", _tempOffsetBuffer);
        
        // Run voxel data generator.
        chunkDataComputeShader.Dispatch(0, threadsX, threadsY, threadsZ);
        chunkDataComputeShader.Dispatch(1, 1, 1, 1);

        _visibleVoxelBuffer.GetData(visibleVoxelArray);

        _meshBuffer?.Release();

        // Mesh buffer on the GPU.
        // Potentially 12 triangles per voxel (each triangle has 3 verts/norms (each is 3+3+3 floats) for a total of 3*(3*3) floats)
        _meshBuffer = new ComputeBuffer(visibleVoxelArray[0] * 12, sizeof(float) * (9 + 3 + 3), ComputeBufferType.Append);

        // Reset mesh counter for new writes.
        _meshBuffer.SetCounterValue(0);
        
        // Assign buffers.
        chunkMeshComputeShader.SetBuffer(0, "MeshBuffer", _meshBuffer);
        chunkMeshComputeShader.SetBuffer(1, "DrawArgsBuffer", _drawArgsBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        
        // Run voxel mesh generator.
        chunkMeshComputeShader.Dispatch(0, threadsX, threadsY, threadsZ);
        
        // Copy appendBuffer count over to vertexCountPerInstance (it is at byte offset 0)
        // However, each entry in the buffer is 1 triangle, so the actual value is 3x greater.
        ComputeBuffer.CopyCount(_meshBuffer, _drawArgsBuffer, 0);

        // Multiplies the vertexCountPerInstance by 3
        chunkMeshComputeShader.Dispatch(1, 1, 1, 1);
        
        // Prepare mesh draw pass.
        drawMeshMaterial.SetBuffer("MeshBuffer", _meshBuffer);
    }

    // Update is called once per frame
    void Update()
    {
        if(update)
            UpdateBuffers();

        Graphics.DrawProceduralIndirect(
            drawMeshMaterial,
            new Bounds(transform.position, transform.lossyScale),
            MeshTopology.Triangles,
            _drawArgsBuffer,
            0
        );
    }
}
