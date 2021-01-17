using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Types;
using Random = System.Random;

namespace VoxelEngine.Data
{
    [CreateAssetMenu(fileName = "TerrainDataGenerator.asset", menuName = "VoxelEngine/Data Points/Terrain Data Generator")]
    public class TerrainVoxelDataGenerator : VoxelDataGeneratorBase
    {
        public float offset = 1;
        public float height = 0;
        public float heightScale = 1;

        public float hillsScale1 = 0.1f;
        public float hillsScale2 = 0.1f;
        public float hillsScale3 = 0.1f;

        public float scale1 = 0.1f;
        public float scale2 = 0.05f;
        public float scale3 = 0.025f;
        
        public float intensity1 = 0.1f;
        public float intensity2 = 0.05f;
        public float intensity3 = 0.025f;
        
        public override float CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            var hills1 = 1;

            return 1;
        }
        
        float PerlinTest3D(float x, float y, float z)
        {
            float intensity = Mathf.PerlinNoise(x, y);
            intensity += Mathf.PerlinNoise(y, z);
            intensity += Mathf.PerlinNoise(x, z);
            
            intensity += Mathf.PerlinNoise(y, x);
            intensity += Mathf.PerlinNoise(z, y);
            intensity += Mathf.PerlinNoise(z, x);

            return intensity / 6.0f;
        }
    }
}
