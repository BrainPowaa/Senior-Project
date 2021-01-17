using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public struct ChunkData
    {
        public readonly VoxelData[] voxels;

        public ChunkData(VoxelData[] voxels)
        {
            this.voxels = voxels;
        }
    }
}