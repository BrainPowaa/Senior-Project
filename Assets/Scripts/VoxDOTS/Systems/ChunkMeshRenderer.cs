using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using VoxDOTS.Data;
using VoxDOTS.Tags;

namespace VoxDOTS.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    //[UpdateAfter(typeof(ChunkMeshGenerator))]
    public class ChunkMeshRenderer : SystemBase
    {
        private Dictionary<int, Mesh> _entityToMeshObject;
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

        private Material _material;

        private EntityQuery _addMeshObjectTagQuery;
        private EntityQuery _removeMeshObjectTagQuery;
        private EntityQuery _chunkRenderedQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            _material = new Material(Shader.Find("HDRP/Lit"));

            _entityToMeshObject = new Dictionary<int, Mesh>();
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            // TODO: Use the Hybrid Renderer as intended, but its too terrible and slow currently.

            var ecb = _entityCommandBufferSystem.CreateCommandBuffer();

            // TEMP: Add chunk entity to dictionary
            Entities
                .WithName("Vox_Render_AddEntitySharedGameObjectToDictionary")
                .WithStoreEntityQueryInField(ref _addMeshObjectTagQuery)
                .WithoutBurst()
                .WithAll<ChunkMeshVerticesData, ChunkMeshIndicesData>()
                .WithNone<ChunkMeshObjectTag>()
                .ForEach((Entity entity) =>
                {
                    Debug.Log("Added mesh");

                    Mesh mesh = new UnityEngine.Mesh();

                    mesh.MarkDynamic();
                    
                    _entityToMeshObject.Add(entity.Index, mesh);
                    ecb.AddComponent<ChunkMeshObjectTag>(entity);
                    ecb.AddComponent<ChunkMeshNeedsRenderTag>(entity);
                }).Run();
            
            // TEMP: Clean-up chunks and remove from dictionary
            Entities
                .WithName("Vox_Render_CleanupDeadChunks")
                .WithStoreEntityQueryInField(ref _removeMeshObjectTagQuery)
                .WithoutBurst()
                .WithNone<ChunkMeshVerticesData, ChunkMeshIndicesData>()
                .WithAll<ChunkMeshObjectTag>()
                .ForEach((Entity entity) =>
                {
                    Debug.Log("Remove mesh");

                    Object.Destroy(_entityToMeshObject[entity.Index]);

                    _entityToMeshObject.Remove(entity.Index);
                    ecb.RemoveComponent<ChunkMeshObjectTag>(entity);

                }).Run();
            
            // Update Chunk mesh with pre-generated buffer.
            Entities
                .WithName("Vox_Render_UpdateChunkMeshBuffer")
                .WithoutBurst()
                .WithAll<ChunkMeshNeedsRenderTag, ChunkMeshObjectTag>()
                .ForEach((in Entity entity, in DynamicBuffer<ChunkMeshVerticesData> verticesBuffer, in DynamicBuffer<ChunkMeshIndicesData> indicesBuffer) =>
                {
                    Debug.Log("Ye");
                    var mesh = _entityToMeshObject[entity.Index];
                    
                    mesh.Clear();

                    var vertices = verticesBuffer.Reinterpret<float3>().AsNativeArray();
                    var indices = indicesBuffer.Reinterpret<int>().AsNativeArray();

                    mesh.SetVertexBufferParams(
                        vertices.Length,
                        new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3)
                    );
                    
                    mesh.SetVertexBufferData(vertices, 0, 0, vertices.Length, 0,
                        MeshUpdateFlags.DontRecalculateBounds
                        | MeshUpdateFlags.DontValidateIndices);
                    
                    mesh.SetIndexBufferParams(indices.Length, IndexFormat.UInt32);
                    mesh.SetIndexBufferData(indices, 0, 0, indices.Length,
                        MeshUpdateFlags.DontRecalculateBounds
                        | MeshUpdateFlags.DontValidateIndices);
                    
                    mesh.SetSubMesh(0, new SubMeshDescriptor(0, vertices.Length));
                    
                    ecb.RemoveComponent<ChunkMeshNeedsRenderTag>(entity);
                }).Run();

            Entities
                .WithoutBurst()
                .WithAll<ChunkMeshObjectTag, ChunkMeshIndicesData, ChunkMeshVerticesData>()
                .ForEach((Entity entity) =>
                {
                    Graphics.DrawMesh
                    (
                        _entityToMeshObject[entity.Index],
                        Vector3.zero,
                        Quaternion.identity,
                        _material,
                        0, null, 0, null, true, true, true
                    );
                }).Run();
            
            //ecb.AddComponent<ChunkMeshObjectTag>(_addMeshObjectTagQuery);
            //ecb.RemoveComponent<ChunkMeshObjectTag>(_removeMeshObjectTagQuery);
            //ecb.RemoveComponent<ChunkMeshNeedsRenderTag>(_chunkRenderedQuery);

            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
