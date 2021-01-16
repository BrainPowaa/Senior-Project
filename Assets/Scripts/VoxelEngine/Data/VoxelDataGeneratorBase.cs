using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Mesh;
using VoxelEngine.Types;
using Random = UnityEngine.Random;

namespace VoxelEngine.Data
{
    public abstract class VoxelDataGeneratorBase : ScriptableObject
    {
        public int seed = 0;
        public float scale = 1;

        public void InitializeGenerator()
        {
            Random.InitState(seed);
        }

        void OnValidate()
        {
            foreach (var chunkComponent in FindObjectsOfType<VoxelChunkComponent>())
            {
                chunkComponent.GenerateMesh();
            }
        }

        public virtual ChunkData CreateChunkData(Vector3Int chunkPosition)
        {
            int maxChunkSize =
                VoxelEngineConstant.ChunkSize *
                VoxelEngineConstant.ChunkSize *
                VoxelEngineConstant.ChunkSize;

            List<VoxelData> voxels = new List<VoxelData>();
            
            for (int i = 0; i < maxChunkSize; i++)
            {
                var x = i % VoxelEngineConstant.ChunkSize;
                var z = (i / VoxelEngineConstant.ChunkSize) % VoxelEngineConstant.ChunkSize;
                var y = i / VoxelEngineConstant.ChunkSize / VoxelEngineConstant.ChunkSize;
                
                var position = new Vector3Int
                (
                    x + (chunkPosition.x * VoxelEngineConstant.ChunkSize),
                    y + (chunkPosition.y * VoxelEngineConstant.ChunkSize),
                    z + (chunkPosition.z * VoxelEngineConstant.ChunkSize)
                );

                voxels.Add(CreateVoxelData(position));
            }

            return new ChunkData(voxels);
        }
        public abstract VoxelData CreateVoxelData(Vector3Int position);
    }
}
