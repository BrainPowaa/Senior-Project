using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using VoxDOTS;

public class ComputeMeshTest : MonoBehaviour
{
    public Material drawMeshMaterial;
    public ComputeShader chunkMeshShader;

    private ComputeBuffer _chunkDataBuffer, _meshBuffer, _drawCallArgs;

    private NativeArray<int> _testChunkData;
    private static readonly int _meshBufferName = Shader.PropertyToID("MeshBuffer");

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
        _testChunkData = new NativeArray<int>(4, Allocator.Persistent);

        _testChunkData[0] = 16;
        _testChunkData[1] = 4;
        _testChunkData[2] = 16;
        _testChunkData[3] = 4;

        _chunkDataBuffer = new ComputeBuffer(4, sizeof(int));

        _drawCallArgs = new ComputeBuffer(1, sizeof(int) * 4);
        _drawCallArgs.SetData(new[] { new DrawArg { vertexCountPerInstance = 3 } });

        // There are 3 floats per triangle, 12 tris per cube (potentially) 
        _meshBuffer = new ComputeBuffer(Constants.MaxChunkSize, sizeof(float) * 3 * 12);
    }

    private void OnDisable()
    {
        _testChunkData.Dispose();
        _chunkDataBuffer.Dispose();
        _drawCallArgs.Dispose();
        _meshBuffer.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        _chunkDataBuffer.SetData(_testChunkData);
        
        chunkMeshShader.SetBuffer(0, _meshBufferName, _chunkDataBuffer);
        
        drawMeshMaterial.SetBuffer(_meshBufferName, _chunkDataBuffer);

        Graphics.DrawProceduralIndirect(
            drawMeshMaterial,
            new Bounds(),
            MeshTopology.Triangles,
            _drawCallArgs,
            0
        );
    }
}
