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
                chunkComponent.RefreshChunk();
            }
        }

        public virtual ChunkData CreateChunkData(Vector3Int chunkPosition)
        {
            var voxels = new VoxelData[VoxelEngineConstant.MaxChunkSize];
            
            for (int i = 0; i < VoxelEngineConstant.MaxChunkSize; i++)
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

                var intensity = (byte)(CreateVoxelData(position) * 15);
                
                voxels[i].info = VoxelData.PackData(intensity, 0);
            }

            return new ChunkData(voxels);
        }
        
        public virtual void RefreshChunkData(ref ChunkData chunkData, Vector3Int chunkPosition)
        {
            for (int i = 0; i < VoxelEngineConstant.MaxChunkSize; i++)
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

                var intensity = (byte)(CreateVoxelData(position) * 15);
                
                chunkData.voxels[i].info = VoxelData.PackData(intensity, 0);
            }
        }
        
        public abstract float CreateVoxelData(Vector3Int position);
    }
}
