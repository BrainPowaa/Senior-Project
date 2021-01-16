using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelEngine.Data;
using VoxelEngine.Mesh;
using VoxelEngine.Types;

namespace VoxelEngine.Mesh
{
    [ExecuteAlways]
    public class VoxelChunkComponent : MonoBehaviour
    {
        public VoxelChunkMeshGeneratorBase meshGenerator;
        public VoxelDataGeneratorBase dataGenerator;
        public Vector3Int position;
        public Material material;

        private byte ChunkSize = 16;

        private MeshRenderer _meshRenderer;
        private UnityEngine.Mesh _mesh;
        private MeshFilter _meshFilter;
    
        // Start is called before the first frame update
        private void Start()
        {
            if (meshGenerator != null && dataGenerator != null && material != null)
            {
                Init();
            }
        }

        void OnValidate()
        {
            GenerateMesh();
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
            
            GenerateMesh();
        }
        
        public void GenerateMesh()
        {
            dataGenerator.InitializeGenerator();

            var chunk = dataGenerator.CreateChunkData(position);

            VoxelMeshRenderData meshData = meshGenerator.GenerateChunkMesh(chunk, position);
            
            _mesh.Clear();
            
            _mesh.SetVertices(meshData.vertices);
            _mesh.SetTriangles(meshData.triangles, 0);
            
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }
    }
}