using System;
using Unity.Entities;
using Unity.Mathematics;
using VoxDOTS.Components;
using VoxDOTS.Data;
using VoxDOTS.Tags;
using Random = System.Random;

namespace VoxDOTS.Systems
{
    [UpdateBefore(typeof(ChunkMeshGenerator))]
    public class ChunkDataNoiseGenerator : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        private EntityQuery _entityQuery;
        
        private Unity.Mathematics.Random _random;

        protected override void OnCreate()
        {
            base.OnCreate();

            _random = new Unity.Mathematics.Random(12321);
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _entityCommandBufferSystem.CreateCommandBuffer();

            var myRand = _random;
            
            Entities
                .WithName("Vox_Noise_CreateVoxelNoiseData")
                .WithStoreEntityQueryInField(ref _entityQuery)
                .WithBurst()
                .WithAll<ChunkDataNoiseGeneratorTag, ChunkNeedsVoxelDataTag>()
                .ForEach((ref DynamicBuffer<ChunkData> chunkDataBuffer, in Entity entity, in Chunk chunk) =>
                {
                    chunkDataBuffer.Clear();
                    
                    for (int i = 0; i < Constants.MaxChunkSize; i++)
                    {
                        chunkDataBuffer.Add(new ChunkData((byte)myRand.NextInt(0, 15)));
                    }
                }).ScheduleParallel();
            
            ecb.AddComponent<ChunkMeshDirtyTag>(_entityQuery);
            ecb.RemoveComponent<ChunkNeedsVoxelDataTag>(_entityQuery);
            
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
        
        public static float3 GetPositionFromIndex(int index)
        {
            return new float3
            {
                x = index % Constants.ChunkSize,
                y = (index / Constants.ChunkSize) % Constants.ChunkSize,
                z = (index / Constants.ChunkSize / Constants.ChunkSize) % Constants.ChunkSize,
            };
        }
        
        public static int GetIndexFromPosition(float3 position)
        {
            return
                (int)(position.x +
                      position.y * Constants.ChunkSize * Constants.ChunkSize +
                      position.z * Constants.ChunkSize * Constants.ChunkSize);
        }
    }
}