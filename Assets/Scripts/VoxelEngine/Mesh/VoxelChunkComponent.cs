using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using VoxelEngine.Data;
using VoxelEngine.Mesh;
using VoxelEngine.Types;

namespace VoxelEngine.Mesh
{
    [ExecuteAlways]
    public class VoxelChunkComponent : MonoBehaviour
    {
        static ProfilerMarker s_InitPerfMarker = new ProfilerMarker("VoxelEngine.Init");
        static ProfilerMarker s_DataPointPerfMarker = new ProfilerMarker("VoxelEngine.DataPointGen");
        static ProfilerMarker s_MeshGenerationPerfMarker = new ProfilerMarker("VoxelEngine.MeshGeneration");
        
        public VoxelChunkMeshGeneratorBase meshGenerator;
        public VoxelDataGeneratorBase dataGenerator;
        public Vector3Int position;
        public Material material;

        private MeshRenderer _meshRenderer;
        private UnityEngine.Mesh _mesh;
        private MeshFilter _meshFilter;

        private ChunkData _chunkData;
        private bool _isReady;
    
        // Start is called before the first frame update
        private void Start()
        {
            _isReady = false;
            if (meshGenerator != null && dataGenerator != null && material != null)
            {
                Init();
            }
        }

        void OnValidate()
        {
            RefreshChunk();
        }
        
        public void Init()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _mesh = gameObject.GetComponent<UnityEngine.Mesh>();
            
            if (!_meshRenderer)
            {
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            if (!_meshFilter)
            {
                _meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            if (!_mesh)
            {
                _mesh = new UnityEngine.Mesh();
            }

            _meshRenderer.material = material;
            _meshFilter.mesh = _mesh;
            _mesh.indexFormat = IndexFormat.UInt32;
            
            Profiler.BeginSample("My Sample");
            
            dataGenerator.InitializeGenerator();
            _chunkData = dataGenerator.CreateChunkData(position);
            
            Profiler.EndSample();

            _isReady = true;
            
            GenerateMesh();
        }

        public void RefreshChunk()
        {
            if (_isReady)
            {
                Assert.IsNotNull(_chunkData.voxels);

                s_DataPointPerfMarker.Begin();
                
                dataGenerator.RefreshChunkData(ref _chunkData, position);
                
                s_DataPointPerfMarker.End();
                GenerateMesh();
            }
        }
        
        public void GenerateMesh()
        {
            s_MeshGenerationPerfMarker.Begin();
            VoxelMeshRenderData meshData = meshGenerator.GenerateChunkMesh(ref _chunkData, position);
            
            _mesh.Clear();
            
            _mesh.SetVertices(meshData.vertices);
            _mesh.SetTriangles(meshData.triangles, 0);
            
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            s_MeshGenerationPerfMarker.End();
        }
    }
}