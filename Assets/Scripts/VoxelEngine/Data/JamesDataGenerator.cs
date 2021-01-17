using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Types;

namespace VoxelEngine.Data
{
    [CreateAssetMenu(fileName = "JamesDataGenerator.asset", menuName = "VoxelEngine/Data Points/James Data Generator")]
    public class JamesDataGenerator : VoxelDataGeneratorBase
    {
        public float offset = 1;
        private FastNoise Noise = new FastNoise();
        public override VoxelData CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            float intensity = Noise.GetValueFractal(x, y);
            intensity += Noise.GetSimplex(y, z);
            intensity += Noise.GetPerlin(x, z);
            
            intensity += Noise.GetValueFractal(y, x);
            intensity += Noise.GetSimplex(z, y);
            intensity += Noise.GetPerlin(z, x);
            
            intensity /= 6.0f;

            intensity += offset;

            return new VoxelData((byte)(intensity * 16), 0);
        }
    }
}