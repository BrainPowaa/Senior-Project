using Unity.Entities;
using Unity.Mathematics;

namespace VoxDOTS.Data
{
    public struct ChunkMeshData : IBufferElementData
    {
        public float3 Vertices;
        public int Indices;
    }
}