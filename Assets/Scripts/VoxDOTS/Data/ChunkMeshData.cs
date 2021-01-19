using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace VoxDOTS.Data
{
    public struct ChunkMeshData : IBufferElementData
    {
        public float3 Vertices;
        public int Indices;
    }

    [GenerateAuthoringComponent]
    [InternalBufferCapacity(Constants.MaxChunkSize)]
    public struct ChunkData : IBufferElementData
    {
        public byte VoxelData;
    }
}