using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition.Attributes;
using VoxelEngine.Data;

namespace VoxelEngine.Rendering
{
    public struct VoxelChunkMeshRenderData
    {
        public List<Vector3> Verticies;
        public List<int> Triangles;
    }

    abstract class VoxelChunkMeshGeneratorBase : ScriptableObject
    {
        public abstract VoxelChunkMeshRenderData GenerateVoxel(ref VoxelData voxel, VoxelData[] neighbors);
    }
}