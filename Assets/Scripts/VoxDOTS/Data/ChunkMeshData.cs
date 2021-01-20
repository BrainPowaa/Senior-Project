using Unity.Entities;
using Unity.Mathematics;

namespace VoxDOTS.Data
{
    [GenerateAuthoringComponent]
    public struct ChunkMeshData : IBufferElementData
    {
        public float3 Vertices;
        public int Indices;
    }
}