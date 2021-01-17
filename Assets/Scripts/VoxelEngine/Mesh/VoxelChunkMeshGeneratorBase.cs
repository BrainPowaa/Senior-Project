using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition.Attributes;
using VoxelEngine.Data;
using VoxelEngine.Types;
using Vector3 = UnityEngine.Vector3;

namespace VoxelEngine.Mesh
{
    public struct VoxelMeshRenderData
    {
        public readonly List<Vector3> vertices;
        public readonly List<int> triangles;
        
        public VoxelMeshRenderData(List<Vector3> vertices, List<int> triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }
    }

    public abstract class VoxelChunkMeshGeneratorBase : ScriptableObject
    {
        public virtual VoxelMeshRenderData GenerateChunkMesh(ref ChunkData chunkData, Vector3Int chunkPosition)
        {
            VoxelMeshRenderData meshData = new VoxelMeshRenderData(new List<Vector3>(), new List<int>());

            for (int y = 0; y < VoxelEngineConstant.ChunkSize; y++)
            {
                for (int z = 0; z < VoxelEngineConstant.ChunkSize; z++)
                {
                    for (int x = 0; x < VoxelEngineConstant.ChunkSize; x++)
                    {
                        var position = new Vector3Int(x, y, z);

                        var dataPoint = position;
                        dataPoint.x += (chunkPosition.x * VoxelEngineConstant.ChunkSize);
                        dataPoint.y += (chunkPosition.y * VoxelEngineConstant.ChunkSize);
                        dataPoint.z += (chunkPosition.z * VoxelEngineConstant.ChunkSize);

                        var neighbors = new VoxelData[6];

                        var generatedVoxel = GenerateVoxel(dataPoint, ref chunkData.voxels[x][y][z], neighbors);

                        foreach (var triangle in generatedVoxel.triangles)
                        {
                            meshData.triangles.Add(triangle + meshData.vertices.Count);
                        }

                        foreach (var vertex in generatedVoxel.vertices)
                        {
                            meshData.vertices.Add(vertex + position);
                        }
                    }
                }
            }

            return meshData;
        }

        public abstract VoxelMeshRenderData GenerateVoxel(Vector3Int position, ref VoxelData voxel, VoxelData[] neighbors);
    }
}