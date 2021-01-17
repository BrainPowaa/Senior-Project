using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var fullCube = new List<int>()
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

            
            var hiddenFaces = new Dictionary<int, List<int>>();
            hiddenFaces[0] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[1] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[2] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[3] = new List<int>() {3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[4] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[5] = new List<int>() {4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[6] = new List<int>() {1, 3, 5, 7, 5, 3, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[7] = new List<int>() {0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[8] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[9] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[10] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[11] = new List<int>() {3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[12] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[13] = new List<int>() {4, 6, 0, 2, 0, 6, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[14] = new List<int>() {1, 3, 5, 7, 5, 3, 5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[15] = new List<int>() {5, 7, 4, 6, 4, 7, 0, 2, 1, 3, 1, 2};
            hiddenFaces[16] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[17] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[18] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[19] = new List<int>() {3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[20] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[21] = new List<int>() {4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[22] = new List<int>() {1, 3, 5, 7, 5, 3, 0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[23] = new List<int>() {0, 1, 4, 5, 4, 1, 0, 2, 1, 3, 1, 2};
            hiddenFaces[24] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 2, 1, 3, 1, 2};
            hiddenFaces[25] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 2, 1, 3, 1, 2};
            hiddenFaces[26] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 0, 2, 1, 3, 1, 2};
            hiddenFaces[27] = new List<int>() {3, 2, 7, 6, 7, 2, 0, 2, 1, 3, 1, 2};
            hiddenFaces[28] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 0, 2, 1, 3, 1, 2};
            hiddenFaces[29] = new List<int>() {4, 6, 0, 2, 0, 6, 0, 2, 1, 3, 1, 2};
            hiddenFaces[30] = new List<int>() {1, 3, 5, 7, 5, 3, 0, 2, 1, 3, 1, 2};
            hiddenFaces[31] = new List<int>() {0, 2, 1, 3, 1, 2};
            hiddenFaces[32] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[33] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[34] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[35] = new List<int>() {3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[36] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[37] = new List<int>() {4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[38] = new List<int>() {1, 3, 5, 7, 5, 3, 0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[39] = new List<int>() {0, 1, 4, 5, 4, 1, 5, 7, 4, 6, 4, 7};
            hiddenFaces[40] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7};
            hiddenFaces[41] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7};
            hiddenFaces[42] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7};
            hiddenFaces[43] = new List<int>() {3, 2, 7, 6, 7, 2, 5, 7, 4, 6, 4, 7};
            hiddenFaces[44] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 5, 7, 4, 6, 4, 7};
            hiddenFaces[45] = new List<int>() {4, 6, 0, 2, 0, 6, 5, 7, 4, 6, 4, 7};
            hiddenFaces[46] = new List<int>() {1, 3, 5, 7, 5, 3, 5, 7, 4, 6, 4, 7};
            hiddenFaces[47] = new List<int>() {5, 7, 4, 6, 4, 7};
            hiddenFaces[48] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1};
            hiddenFaces[49] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1};
            hiddenFaces[50] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1};
            hiddenFaces[51] = new List<int>() {3, 2, 7, 6, 7, 2, 0, 1, 4, 5, 4, 1};
            hiddenFaces[52] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1};
            hiddenFaces[53] = new List<int>() {4, 6, 0, 2, 0, 6, 0, 1, 4, 5, 4, 1};
            hiddenFaces[54] = new List<int>() {1, 3, 5, 7, 5, 3, 0, 1, 4, 5, 4, 1};
            hiddenFaces[55] = new List<int>() {0, 1, 4, 5, 4, 1};
            hiddenFaces[56] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2};
            hiddenFaces[57] = new List<int>() {4, 6, 0, 2, 0, 6, 3, 2, 7, 6, 7, 2};
            hiddenFaces[58] = new List<int>() {1, 3, 5, 7, 5, 3, 3, 2, 7, 6, 7, 2};
            hiddenFaces[59] = new List<int>() {3, 2, 7, 6, 7, 2};
            hiddenFaces[60] = new List<int>() {1, 3, 5, 7, 5, 3, 4, 6, 0, 2, 0, 6};
            hiddenFaces[61] = new List<int>() {4, 6, 0, 2, 0, 6};
            hiddenFaces[62] = new List<int>() {1, 3, 5, 7, 5, 3};
            hiddenFaces[63] = new List<int>(){};
            
            var index = 0x0;
            for (int i = 0; i < 6; i++)
            {
                if(VoxelData.UnpackData(neighbors[i].info).Item1 >= 8)
                {
                    index |= (byte)(1 << i);
                }
            }
            
            return new VoxelMeshRenderData(vertices, hiddenFaces[index]);
                
        }
    }

}