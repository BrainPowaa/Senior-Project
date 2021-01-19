using System;
using System.Collections;
using System.Collections.Generic;
using LibNoise;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine.Types;

namespace VoxelEngine.Data
{
    [CreateAssetMenu(fileName = "JamesDataGenerator.asset", menuName = "VoxelEngine/Data Points/James Data Generator")]
    
    public class JamesDataGenerator : VoxelDataGeneratorBase
    {
        public float offset = -0.55f;
        public float noiseScale = 0.1f;
        public int octaveCount = 1;
        public float frequency = 2.3f;
        public float persistence = 1;
        public float lacunarity = 1;
        private FastNoise noise = new FastNoise();

        public override float CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            noise.Frequency = frequency;
            noise.Lacunarity = lacunarity;
            noise.Persistence = persistence;
            noise.NoiseQuality = NoiseQuality.Low;
            noise.OctaveCount = octaveCount;
            noise.Seed = seed;

            if (position.y > 128)
            {
                
                float intensity = (float)noise.GetValue(x, y, z) * noiseScale;

                intensity += offset;

                return intensity;
                
            }

            return 0;

        }
    }
}