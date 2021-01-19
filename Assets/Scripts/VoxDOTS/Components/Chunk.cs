using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace VoxDOTS.Components
{
    [GenerateAuthoringComponent]
    public struct Chunk : IComponentData
    {
        public uint3 Position;
    }
}