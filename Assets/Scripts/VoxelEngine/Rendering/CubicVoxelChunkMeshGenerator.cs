using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Data;

namespace VoxelEngine.Rendering
{
    [CreateAssetMenu]
    class CubicVoxelChunkMeshGenerator : VoxelChunkMeshGeneratorBase
    {
        public override VoxelChunkMeshRenderData GenerateVoxel(ref VoxelData voxel, VoxelData[] neighbors)
        {
            // Refer to CubeReference.png
            List<Vector3> vertices = new List<Vector3>()
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0), // X
                new Vector3(0, 1, 0), //   Y
                new Vector3(1, 1, 0), // X Y
                new Vector3(0, 0, 1), //     Z
                new Vector3(1, 0, 1), // X   Z
                new Vector3(0, 1, 1), //   Y Z
                new Vector3(1, 1, 1), // X Y Z
            };

            // Triangles
            List<int> tris = new List<int>()
            {
                // -X face
                4, 6, 0,
                2, 0, 6,

                // +X face
                1, 3, 5,
                7, 5, 3,
                
                // -Z face
                0, 2, 1,
                3, 1, 2,

                // +Z face
                5, 4, 7,
                6, 7, 4,

                // -Y face
                0, 1, 4,
                5, 4, 1,

                // +Y face
                3, 2, 7,
                6, 7, 2,
            };

            return new VoxelChunkMeshRenderData
            {
                Verticies = vertices,
                Triangles = tris
            };
        }
    }

}