using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Data;
using VoxelEngine.Types;

namespace VoxelEngine.Mesh
{
    [CreateAssetMenu(fileName = "CubicGenerator.asset", menuName = "VoxelEngine/Mesh/Cubic Generator")]
    public class CubicVoxelChunkMeshGenerator : VoxelChunkMeshGeneratorBase
    {
        public override VoxelMeshRenderData GenerateVoxel(Vector3Int position, ref VoxelData voxel, VoxelData[] neighbors)
        {
            (byte, byte) data = VoxelData.UnpackData(voxel.info);

            var intensity = data.Item1;
            var materialIndex = data.Item2;
            
            if (intensity < 8)
            {
                return new VoxelMeshRenderData(new List<Vector3>(), new List<int>());
            }

            var x = position.x;
            var y = position.y;
            var z = position.z;
            
            // Refer to CubeReference.png
            var vertices = new List<Vector3>()
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
            var tris = new List<int>()
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
                5, 7, 4,
                6, 4, 7,

                // -Y face
                0, 1, 4,
                5, 4, 1,

                // +Y face
                3, 2, 7,
                6, 7, 2,
            };

            return new VoxelMeshRenderData(vertices, tris);
        }
    }

}