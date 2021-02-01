using Unity.Entities;
using Unity.Mathematics;
using VoxDOTS.Components;
using VoxDOTS.Data;
using VoxDOTS.Tags;
using static VoxDOTS.Systems.ChunkDataNoiseGenerator;

namespace VoxDOTS.Systems
{
    //[UpdateInGroup(typeof(PresentationSystemGroup))]
    public class ChunkMeshGenerator : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        EntityQuery _dirtyChunkQuery;
        
        protected override void OnCreate()
        {
            base.OnCreate();

            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _entityCommandBufferSystem.CreateCommandBuffer();

            Entities
                .WithName("Vox_Mesh_ChunkMeshGenerationUpdate")
                .WithAll<ChunkMeshDirtyTag>()
                .WithStoreEntityQueryInField(ref _dirtyChunkQuery)
                .WithBurst()
                .ForEach((ref Entity entity, ref DynamicBuffer<ChunkMeshVerticesData> verticesBuffer,
                    ref DynamicBuffer<ChunkMeshIndicesData> indicesBuffer, in Chunk chunk, in DynamicBuffer<ChunkData> chunkData) =>
                {
                    verticesBuffer.Clear();
                    indicesBuffer.Clear();

                    var vertices = verticesBuffer.Reinterpret<float3>();
                    var indices = indicesBuffer.Reinterpret<int>();
                    
                    for (int i = 0; i < chunkData.Length; i++)
                    {
                        if (chunkData[i].Value < 8)
                        {
                            continue;
                        }

                        var p = GetPositionFromIndex(i) + chunk.Position;
                            
                        if(p.z < Constants.ChunkSize - 1)
                            GenerateFace(ref vertices, ref indices, new int3(0, 0, -1), p);
                        
                        if(p.z > 0)
                            GenerateFace(ref vertices, ref indices, new int3(0, 0, 1), p);

                        if(p.y < Constants.ChunkSize - 1)
                            GenerateFace(ref vertices, ref indices, new int3(0, 1, 0), p);

                        if(p.y > 0)
                            GenerateFace(ref vertices, ref indices, new int3(0, -1, 0), p);

                        if(p.x < Constants.ChunkSize - 1)
                            GenerateFace(ref vertices, ref indices, new int3(1, 0, 0), p);

                        if (p.x > 0)
                            GenerateFace(ref vertices, ref indices, new int3(-1, 0, 0), p);
                    }
                }).Schedule();
            
            ecb.RemoveComponent<ChunkMeshDirtyTag>(_dirtyChunkQuery);
            ecb.AddComponent<ChunkMeshNeedsRenderTag>(_dirtyChunkQuery);
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }

        static void GenerateFace(
            ref DynamicBuffer<float3> vertices, 
            ref DynamicBuffer<int> indices, 
            int3 normal, 
            float3 position)
        {
            
             float3 up = new float3(0, 1, 0);
            if (normal.y != 0)
                up = new float3(-1, 0, 0);

            float3 perp1 = math.cross(normal, up);
            float3 perp2 = math.cross(perp1, normal);
            
            var vertLength = vertices.Length;
            
            vertices.Add(perp2 + position);
            vertices.Add(perp1 + perp2 + position);
            vertices.Add(float3.zero + position);
            vertices.Add(perp1 + position);

            indices.Add(0 + vertLength);
            indices.Add(1 + vertLength);
            indices.Add(2 + vertLength);
            indices.Add(3 + vertLength);
            indices.Add(2 + vertLength);
            indices.Add(1 + vertLength);
        }
    }
}