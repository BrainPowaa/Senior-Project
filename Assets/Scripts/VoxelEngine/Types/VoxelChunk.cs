using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public struct ChunkData
    {
        public readonly VoxelData[][][] voxels;

        public ChunkData(VoxelData[][][] voxels)
        {
            this.voxels = voxels;
        }

        public static VoxelData[][][] CreateVoxelArray()
        {
            var data = new VoxelData[VoxelEngineConstant.ChunkSize][][];

            for (int i = 0; i < VoxelEngineConstant.ChunkSize; i++)
            {
                data[i] = new VoxelData[VoxelEngineConstant.ChunkSize][];

                for (int j = 0; j < VoxelEngineConstant.ChunkSize; j++)
                {
                    data[i][j] = new VoxelData[VoxelEngineConstant.ChunkSize];
                }
            }

            return data;
        }
    }
}