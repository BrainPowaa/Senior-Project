using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Types;

namespace VoxelEngine.Data
{
    [CreateAssetMenu(fileName = "SimpleDataGenerator.asset", menuName = "VoxelEngine/Data Points/Simple Data Generator")]
    public class SimpleVoxelDataGenerator : VoxelDataGeneratorBase
    {
        public float offset = 1;
        public override VoxelData CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            float intensity = Mathf.PerlinNoise(x, y);
            intensity += Mathf.PerlinNoise(y, z);
            intensity += Mathf.PerlinNoise(x, z);
            
            intensity += Mathf.PerlinNoise(y, x);
            intensity += Mathf.PerlinNoise(z, y);
            intensity += Mathf.PerlinNoise(z, x);
            
            intensity /= 6.0f;

            intensity += offset;

            return new VoxelData
            (
                (byte)(intensity * 16),
                0
            );
        }
    }
}
