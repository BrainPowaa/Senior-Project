using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Data;
using VoxelEngine.Rendering;

namespace VoxelEngine.Rendering
{
    [ExecuteAlways]
    class VoxelChunkRenderer : MonoBehaviour
    {
        public VoxelChunkMeshGeneratorBase generator;
    }
}