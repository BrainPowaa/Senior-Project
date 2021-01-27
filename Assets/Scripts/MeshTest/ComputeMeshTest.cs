using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using VoxDOTS;

public class ComputeMeshTest : MonoBehaviour
{
    ComputeShader meshGeneratorTest;
    Material material;

    public ComputeShader drawMeshBuffer;
    public ComputeShader chunkMeshBuffer;
    
    private ComputeBuffer _chunkDataBuffer, _meshBuffer, _drawCallArgs;

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
        _testChunkData = new NativeArray<int>(4, Allocator.Persistent);

        _testChunkData[0] = 16;
        _testChunkData[1] = 4;
        _testChunkData[2] = 16;
        _testChunkData[3] = 4;

        _chunkDataBuffer = new ComputeBuffer(4, sizeof(int));
        
        _drawCallArgs = new ComputeBuffer(1, sizeof(int) * 4);
        _drawCallArgs.SetData(new[] {new DrawArg {vertexCountPerInstance = 3}});
        
        _meshBuffer = new ComputeBuffer(Constants.MaxChunkSize, )
    }

    private void OnDisable()
    {
        _testChunkData.Dispose();
        _chunkDataBuffer.Dispose();
        _drawCallArgs.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        _chunkDataBuffer.SetData(_testChunkData);
        
        drawMeshBuffer.SetBuffer(0, "MeshBuffer", _chunkDataBuffer);

        Graphics.DrawProceduralIndirect(
            material, 
            new Bounds(), 
            MeshTopology.Triangles,
            _drawCallArgs, 
            0
        );
    }
}
