using Unity.Entities;
using UnityEngine;
using VoxDOTS.Data;
using VoxDOTS.Tags;

namespace VoxDOTS.Systems.Mesh
{
    public class ChunkMeshGenerator : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithName("Vox_ChunkMeshRender_Update")
                .WithAll<DirtyChunkTag>()
                .WithBurst()
                .ForEach((ref Entity entity) =>//, ref DynamicBuffer<ChunkMeshData> meshData, in DynamicBuffer<ChunkData> chunkData) =>
                {
                    //EntityManager.RemoveComponent<DirtyChunkTag>(entity);
                    Debug.Log("TickEntity");
                    var a = 0;
                    
                    for (int i = 0; i < 10000; i++)
                    {
                        a++;
                    }
                }).ScheduleParallel();
        }
    }
}