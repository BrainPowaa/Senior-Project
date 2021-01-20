using System;
using System.Collections;
using System.Collections.Generic;
using LibNoise;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using VoxelEngine.Types;
using Math = System.Math;

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
        
        public int surfaceBeginLevel = 64;
        public float hillScale = 0.001f;
        public float hillCutoffScale = 0.1f;
        
        public float rmfNoiseScale = 0.01f;
        public float rmfHillScale = 2;
        public float rmfDisplacement = 1;
        public float rmfFrequency = 0.0001f;
        public int rmfOctaveCount = 9;
        
        private Perlin noise = new Perlin();
        private Perlin rmfNoise = new Perlin();


        public override float CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;
            
            //Adjust settings of the noise function to simulate terrain in a more realistic manner
            noise.Frequency = frequency;
            noise.Lacunarity = lacunarity;
            noise.Persistence = persistence;
            noise.NoiseQuality = NoiseQuality.Low;
            noise.OctaveCount = octaveCount;
            noise.Seed = seed;

            rmfNoise.Seed = seed/2;
            rmfNoise.Frequency = rmfFrequency;
            rmfNoise.NoiseQuality = NoiseQuality.Low;
            rmfNoise.OctaveCount = rmfOctaveCount;
            
            // Hills
            var value = (((float) noise.GetValue(x, surfaceBeginLevel, z) * hillScale) -
                         ((position.y - surfaceBeginLevel) * hillCutoffScale));
            value += (((float) rmfNoise.GetValue(x, surfaceBeginLevel, z) * rmfHillScale) -
                      ((position.y - surfaceBeginLevel) * rmfNoiseScale));
            value = Math.Min(Math.Max(value, -1.0f), 1.0f);
            
            // Caves
            value -= ((float)noise.GetValue(x, y, z) * noiseScale) + offset;

            return value;

        }
    }
}