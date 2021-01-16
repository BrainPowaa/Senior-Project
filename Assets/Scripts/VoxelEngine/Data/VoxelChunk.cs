using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Data
{
    public struct Chunk
    {
        readonly List<VoxelData> Voxels;

        Chunk(List<VoxelData> voxels)
        {
            Voxels = voxels;
        }
    }
}