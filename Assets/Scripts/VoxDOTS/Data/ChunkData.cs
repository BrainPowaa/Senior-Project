using Unity.Entities;

namespace VoxDOTS.Data
{
    [InternalBufferCapacity(Constants.MaxChunkSize)]
    public struct ChunkData : IBufferElementData
    {
        public byte Value;

        public ChunkData(byte value) => (Value) = (value);
    }
}