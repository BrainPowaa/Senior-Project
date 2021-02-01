using Unity.Entities;

namespace VoxDOTS.Data
{
    public struct ChunkMeshIndicesData : IBufferElementData
    {
        public int Value;
        
        public ChunkMeshIndicesData(int value) => Value = value;
    }
}