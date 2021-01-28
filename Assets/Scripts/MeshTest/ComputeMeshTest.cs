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
    
    private ComputeBuffer _chunkDataBuffer, _meshBuffer, _drawArgsBuffer;

    private NativeArray<int> _testChunkData;

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
        // TEMP: Native arrays are awesome.
        _testChunkData = new NativeArray<int>(4, Allocator.Persistent);

        // TEMP: Voxel count, use actual constants later.
        var voxelCount = Constants.MaxChunkSize;

        // Voxel data buffer on the GPU.
        _chunkDataBuffer = new ComputeBuffer(voxelCount, sizeof(int));
        
        // Create buffer that has draw args that will be assigned on the GPU.
        _drawArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);
        _drawArgsBuffer.SetData(new[] {new DrawArg {vertexCountPerInstance = 0, instanceCount = 1}});
        
        // Mesh buffer on the GPU.
        // 12 triangles per voxel (each triangle/stride has 3 floats)
        _meshBuffer = new ComputeBuffer(voxelCount * 12, sizeof(float) * 3, ComputeBufferType.Append);
    }

    private void OnDisable()
    {
        // Cleanup GPU / Native buffers.
        _testChunkData.Dispose();
        _chunkDataBuffer.Release();
        _drawArgsBuffer.Release();
        _meshBuffer.Release();
    }

    // Update is called once per frame
    void Update()
    {
        // Test live update.
        _chunkDataBuffer.SetData(_testChunkData);
        
        // Reset mesh counter for new writes.
        _meshBuffer.SetCounterValue(0);
        
        // Assign buffers.
        chunkMeshComputeShader.SetBuffer(0, "MeshBuffer", _meshBuffer);
        chunkMeshComputeShader.SetBuffer(1, "DrawArgsBuffer", _drawArgsBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        
        // Run voxel mesh generator.
        chunkMeshComputeShader.Dispatch(0, 1, 1, 1);
        
        // Copy appendBuffer count over to vertexCountPerInstance (it is at byte offset 0)
        // However, each entry in the buffer is 1 triangle, so the actual value is 3x greater.
        ComputeBuffer.CopyCount(_meshBuffer, _drawArgsBuffer, 0);
        
        // Multiplies the vertexCountPerInstance by 3
        chunkMeshComputeShader.Dispatch(1, 1, 1, 1);
        
        // Prepare mesh draw pass.
        drawMeshMaterial.SetPass(0);
        drawMeshMaterial.SetBuffer("MeshBuffer", _meshBuffer);

        Graphics.DrawProceduralIndirect(
            drawMeshMaterial, 
            new Bounds(transform.position, transform.lossyScale), 
            MeshTopology.Triangles,
            _drawArgsBuffer, 
            0
        );
    }
}
