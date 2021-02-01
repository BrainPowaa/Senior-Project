using System.Drawing;
using System.Numerics;
using Unity.Entities;
using Unity.Mathematics;

namespace VoxDOTS.Data
{
    [InternalBufferCapacity(Constants.MaxChunkSize)]
    public struct ChunkData : IBufferElementData
    {
        public byte Value;

        public ChunkData(byte value) => (Value) = (value);
    }
}

public struct Voxel
{
    public byte Value;
}