using Unity.Entities;
using UnityEngine;
using VoxDOTS.Data;
using VoxDOTS.Tags;
using VoxelEngine.Types;
using ChunkData = VoxDOTS.Data.ChunkData;

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
                .ForEach((ref Entity entity, in DynamicBuffer<ChunkData> chunkData) =>
                {
                    //EntityManager.RemoveComponent<DirtyChunkTag>(entity);
                    Debug.Log("TickEntity");

                    foreach (var data in chunkData)
                    {
                        Debug.Log(data.VoxelData);
                    }
                }).ScheduleParallel();
        }
    }
}