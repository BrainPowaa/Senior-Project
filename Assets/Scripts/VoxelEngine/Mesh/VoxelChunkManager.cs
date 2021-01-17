using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Data;
using VoxelEngine.Mesh;
using VoxelEngine.Types;

namespace VoxelEngine.Mesh
{
    [ExecuteAlways]
    public class VoxelChunkManager : MonoBehaviour
    {
        public VoxelChunkMeshGeneratorBase meshGenerator;
        public VoxelDataGeneratorBase dataGenerator;
        public Vector3Int numberOfChunks = Vector3Int.one;
        public Material material;

        private List<VoxelChunkComponent> _chunks = new List<VoxelChunkComponent>();
        private bool _modifiedSettings = false;
        
        // Start is called before the first frame update
        void Start()
        {
            for (var x = 0; x < numberOfChunks.x; x++)
            {
                for (var y = 0; y < numberOfChunks.y; y++)
                {
                    for (var z = 0; z < numberOfChunks.z; z++)
                    {
                        AddChunk(new Vector3Int(x, y, z));
                    }
                }
            }
        }

        private void Update()
        {
            if (_modifiedSettings)
            {
                _modifiedSettings = false;
                
                ChangeNumberOfChunks();
            }
        }

        private void OnValidate()
        {
            _modifiedSettings = true;
        }

        void AddChunk(Vector3Int position)
        {
            var chunkObject = new GameObject();
            chunkObject.transform.parent = this.transform;
            chunkObject.transform.position = position * VoxelEngineConstant.ChunkSize;

            var chunk = chunkObject.AddComponent<VoxelChunkComponent>();

            chunk.material = material;

            chunk.dataGenerator = dataGenerator;
            chunk.meshGenerator = meshGenerator;
            
            chunk.position = position;
            
            _chunks.Add(chunk);

            chunk.Init();
        }
        
        void ChangeNumberOfChunks()
        {
            for (var x = 0; x < numberOfChunks.x; x++)
            {
                for (var y = 0; y < numberOfChunks.y; y++)
                {
                    for (var z = 0; z < numberOfChunks.z; z++)
                    {
                        var position = new Vector3Int(x, y, z);
                        bool chunkFound = false;

                        for (int i = 0; i < _chunks.Count; i++)
                        {
                            var chunk = _chunks[i];

                            if (chunk.position == position)
                            {
                                chunkFound = true;
                                chunk.RefreshChunk();

                                break;
                            }
                            
                            if (chunk.position.x > numberOfChunks.x ||
                                chunk.position.y > numberOfChunks.y ||
                                chunk.position.z > numberOfChunks.z)
                            {
                                DestroyImmediate(chunk.gameObject);
                                _chunks.RemoveAt(i);

                                i--;
                            }
                            
                        }

                        if (!chunkFound)
                            AddChunk(position);
                    }
                }
            }
        }
    }
}