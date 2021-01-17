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
        
        public override float CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            float intensity = Noise.GetSimplexFractal(x, y, z);

            intensity += offset;

            return intensity;
        }
    }
}