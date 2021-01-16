using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public struct ChunkData
    {
        public readonly List<VoxelData> Voxels;

        public ChunkData(List<VoxelData> voxels)
        {
            Voxels = voxels;
        }
    }
}