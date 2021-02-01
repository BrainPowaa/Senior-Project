using Unity.Entities;
using Unity.Mathematics;

namespace VoxDOTS.Data
{
    public struct ChunkMeshVerticesData : IBufferElementData
    {
        public float3 Value;
        
        public ChunkMeshVerticesData(float3 value) => Value = value;

    }
}