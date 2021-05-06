using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
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
    public ComputeShader voxelVisibilityComputeShader;
    public int lod = 0;
    public int chunkSize = 64;

    public bool update = true;

    public Vector3Int chunkCount = new Vector3Int(1, 1, 1);

    private ComputeBuffer
        _chunkDataBuffer,
        _triangleCountBuffer,
        _meshBuffer,
        _drawArgsBuffer,
        _tempOffsetBuffer,
        _chunkSizeBuffer,
        _chunkLODBuffer;

    private int[] triangleCount = new []{0};

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
        long maxVoxelCount = chunkSize * chunkSize * chunkSize * chunkCount.x * chunkCount.y * chunkCount.z;

        _chunkSizeBuffer = new ComputeBuffer(3, sizeof(uint));
        _chunkSizeBuffer.SetData(new[]
        {
            (uint)(chunkSize * chunkCount.x),
            (uint)(chunkSize * chunkCount.y),
            (uint)(chunkSize * chunkCount.z),
        });

        // Voxel data buffer on the GPU.
        _chunkDataBuffer = new ComputeBuffer((int)maxVoxelCount, sizeof(int));

        _triangleCountBuffer = new ComputeBuffer(1, sizeof(int));
        _triangleCountBuffer.SetData(triangleCount);

        // Create buffer that has draw args that will be assigned on the GPU.
        _drawArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);
        _drawArgsBuffer.SetData(new[] {new DrawArg {vertexCountPerInstance = 0, instanceCount = 1}});

        _tempOffsetBuffer = new ComputeBuffer(1, sizeof(float));
        _chunkLODBuffer = new ComputeBuffer(1, sizeof(uint));

        UpdateBuffers();
    }

    private void OnDisable()
    {
        _chunkDataBuffer.Release();
        _triangleCountBuffer.Release();
        _drawArgsBuffer.Release();
        _meshBuffer?.Release();
        _tempOffsetBuffer.Release();
        _chunkSizeBuffer.Release();
        _chunkLODBuffer.Release();
    }

    void UpdateBuffers()
    {
        var threadsX = (chunkSize / 8) * chunkCount.x / (1 << lod);
        var threadsY = (chunkSize / 8) * chunkCount.y / (1 << lod);
        var threadsZ = (chunkSize / 8) * chunkCount.z / (1 << lod);
        
        // Assign buffers.
        chunkDataComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        chunkDataComputeShader.SetBuffer(0, "Offset", _tempOffsetBuffer);
        chunkDataComputeShader.SetBuffer(0, "ChunkSize", _chunkSizeBuffer);
        chunkDataComputeShader.SetBuffer(0, "ChunkLOD", _chunkLODBuffer);
        chunkDataComputeShader.SetBuffer(1, "Offset", _tempOffsetBuffer);
        
        // Run voxel data generator.
        chunkDataComputeShader.Dispatch(0, threadsX, threadsY, threadsZ);
        chunkDataComputeShader.Dispatch(1, 1, 1, 1);
        
        // Count up visible triangle faces
        _triangleCountBuffer.SetData(new[] {0});
        _chunkLODBuffer.SetData(new[] {(uint)(lod)});

        voxelVisibilityComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        voxelVisibilityComputeShader.SetBuffer(0, "TriangleCount", _triangleCountBuffer);
        voxelVisibilityComputeShader.SetBuffer(0, "ChunkSize", _chunkSizeBuffer);
        voxelVisibilityComputeShader.SetBuffer(0, "ChunkLOD", _chunkLODBuffer);

        // Run counter
        voxelVisibilityComputeShader.Dispatch(0, threadsX, threadsY, threadsZ);
        
        _triangleCountBuffer.GetData(triangleCount);
        
        _meshBuffer?.Release();

        // Mesh buffer on the GPU.
        // Potentially 12 triangles per voxel (each triangle has 3 verts/norms (each is 3+3+3 floats) for a total of 3*(3*3) floats)
        _meshBuffer = new ComputeBuffer(triangleCount[0], sizeof(float) * (3 + 3), ComputeBufferType.Append);

        // Reset mesh counter for new writes.
        _meshBuffer.SetCounterValue(0);
        
        // Assign buffers.
        chunkMeshComputeShader.SetBuffer(0, "MeshBuffer", _meshBuffer);
        chunkMeshComputeShader.SetBuffer(1, "DrawArgsBuffer", _drawArgsBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkSize", _chunkSizeBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkLOD", _chunkLODBuffer);

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
