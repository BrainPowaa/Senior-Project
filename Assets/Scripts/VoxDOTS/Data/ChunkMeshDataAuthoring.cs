using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace VoxDOTS.Data
{
    public class ChunkMeshDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var chunkMeshData = dstManager.AddBuffer<ChunkMeshData>(entity);
            
        }
    }
}