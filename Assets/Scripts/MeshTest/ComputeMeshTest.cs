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
    
    public float offsetSpeed = 0.2f;
    public float scale = 0.5f;
    public float height = 0.5f;
    public float heightScale = 0.5f;

    private ComputeBuffer _chunkDataBuffer, _meshBuffer, _drawArgsBuffer, _tempOffsetBuffer, _fnlStateBuffer;

    private NativeArray<int> _testChunkData;

    private float _offset;
    
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
        _offset = 0f;
        
        // TEMP: Voxel count, use actual constants later.
        var voxelCount = Constants.MaxChunkSize;
        
        // TEMP: Native arrays are awesome.
        _testChunkData = new NativeArray<int>(voxelCount, Allocator.Persistent);

        // Voxel data buffer on the GPU.
        _chunkDataBuffer = new ComputeBuffer(voxelCount, sizeof(int));

        // Create buffer that has draw args that will be assigned on the GPU.
        _drawArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);
        _drawArgsBuffer.SetData(new[] {new DrawArg {vertexCountPerInstance = 0, instanceCount = 1}});
        
        // Mesh buffer on the GPU.
        // Potentially 12 triangles per voxel (each triangle has 3 verts/norms (each is 3+3+4 floats) for a total of 3*(3*2) floats)
        _meshBuffer = new ComputeBuffer(voxelCount * 12, 3 * (sizeof(float) * 9), ComputeBufferType.Append);
        
        // TEMP: Offset buffer
        _tempOffsetBuffer = new ComputeBuffer(1, sizeof(int));
        _tempOffsetBuffer.SetData(new int[] {1});
    }

    private void OnDisable()
    {
        // Cleanup GPU / Native buffers.
        _testChunkData.Dispose();
        _chunkDataBuffer.Release();
        _drawArgsBuffer.Release();
        _meshBuffer.Release();
        _tempOffsetBuffer.Release();
    }

    // Update is called once per frame
    void Update()
    {
        _offset += offsetSpeed * Time.deltaTime;
        /*
        for (int i = 0; i < _testChunkData.Length; i++)
        {
            var x =  i % Constants.ChunkSize;
            var y = (i / Constants.ChunkSize) % Constants.ChunkSize;
            var z =  i / Constants.ChunkSize / Constants.ChunkSize;
            _testChunkData[i] = (int)((8f * Perlin.Noise(x * 0.1f + _offset, 0f, z * 0.1f )) + 8f);
            //_testChunkData[i] = (int)((8f * Perlin.Noise(x * 0.1f + _offset, (y + (Mathf.Sin(_offset * 2)*10)) * 0.1f, z * 0.1f )) + 8f);
            _testChunkData[i] += (int)(((-y * heightScale) - height));
        }
        */
        
        /*
        for (int i = 0; i < _testChunkData.Length; i++)
        {
            var x =  i % Constants.ChunkSize;
            var y = (i / Constants.ChunkSize) % Constants.ChunkSize;
            var z =  i / Constants.ChunkSize / Constants.ChunkSize;
            _testChunkData[i] = (int)((64f * (Perlin.Fbm(x * scale + _offset, 0f, z * scale , 2)) + 1f));
            //_testChunkData[i] = (int)((64f * (Perlin.Noise(x * 0.1f + _offset, (y + (Mathf.Sin(_offset * 2)*10)) * 0.1f, z * 0.1f )) + 1f));
            _testChunkData[i] += (int)(((-y * heightScale) - height));
        }
        
        
        // Test live update.
        _chunkDataBuffer.SetData(_testChunkData);
        */
        
        // Assign buffers.
        chunkDataComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        chunkDataComputeShader.SetBuffer(0, "Offset", _tempOffsetBuffer);
        chunkDataComputeShader.SetBuffer(1, "Offset", _tempOffsetBuffer);
        
        // Run voxel data generator.
        chunkDataComputeShader.Dispatch(0, Constants.ChunkSize / 8, Constants.ChunkSize / 8, Constants.ChunkSize / 8);
        chunkDataComputeShader.Dispatch(1, 1, 1, 1);

        // Reset mesh counter for new writes.
        _meshBuffer.SetCounterValue(0);
        
        // Assign buffers.
        chunkMeshComputeShader.SetBuffer(0, "MeshBuffer", _meshBuffer);
        chunkMeshComputeShader.SetBuffer(1, "DrawArgsBuffer", _drawArgsBuffer);
        chunkMeshComputeShader.SetBuffer(0, "ChunkDataBuffer", _chunkDataBuffer);
        
        // Run voxel mesh generator.
        chunkMeshComputeShader.Dispatch(0, Constants.ChunkSize / 8, Constants.ChunkSize / 8, Constants.ChunkSize / 8);
        
        // Copy appendBuffer count over to vertexCountPerInstance (it is at byte offset 0)
        // However, each entry in the buffer is 1 triangle, so the actual value is 3x greater.
        ComputeBuffer.CopyCount(_meshBuffer, _drawArgsBuffer, 0);
        
        // Multiplies the vertexCountPerInstance by 3
        chunkMeshComputeShader.Dispatch(1, 1, 1, 1);
        
        // Prepare mesh draw pass.
        drawMeshMaterial.SetPass(0);
        drawMeshMaterial.SetBuffer("MeshBuffer", _meshBuffer);

        for(int i = 0; i < 1; i++)
            Graphics.DrawProceduralIndirect(
                drawMeshMaterial,
                new Bounds(transform.position + new Vector3(4.0f, 4.0f, 4.0f), transform.lossyScale),
                MeshTopology.Triangles,
                _drawArgsBuffer,
                0
            );
    }
}
