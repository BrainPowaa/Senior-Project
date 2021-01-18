using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public struct ChunkData
    {
        public readonly byte[][][] voxels;

        public ChunkData(byte[][][] voxels)
        {
            this.voxels = voxels;
        }

        public static byte[][][] CreateVoxelArray()
        {
            var data = new byte[VoxelEngineConstant.ChunkSize][][];

            for (int i = 0; i < VoxelEngineConstant.ChunkSize; i++)
            {
                data[i] = new byte[VoxelEngineConstant.ChunkSize][];

                for (int j = 0; j < VoxelEngineConstant.ChunkSize; j++)
                {
                    data[i][j] = new byte[VoxelEngineConstant.ChunkSize];
                }
            }

            return data;
        }
    }
}