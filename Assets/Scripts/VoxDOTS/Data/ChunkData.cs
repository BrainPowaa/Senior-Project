using Unity.Entities;

namespace VoxDOTS.Data
{
    [GenerateAuthoringComponent]
    [InternalBufferCapacity(Constants.MaxChunkSize)]
    public struct ChunkData : IBufferElementData
    {
        public byte VoxelData;
    }
}