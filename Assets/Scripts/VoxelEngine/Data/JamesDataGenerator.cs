using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using LibNoise;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using VoxelEngine.Types;
using Math = System.Math;
using Perlin = LibNoise.Perlin;

namespace VoxelEngine.Data
{
    [CreateAssetMenu(fileName = "JamesDataGenerator.asset", menuName = "VoxelEngine/Data Points/James Data Generator")]
    
    public class JamesDataGenerator : VoxelDataGeneratorBase
    {
        
        public int surfaceBeginLevel = 80;
        public float hillScale = 0.001f;
        public float secondaryHillScale = 2.0f;
        public float hillCutoffScale = 0.1f;
        public float secondaryHillNoiseScale = 0.01f;
        public float caveNoiseScale = 1f;
        public float caveFrequency = 1f;
        public float caveCutoffLevel = 0.5f;
        public float caveWarpScale = 1.0f;
        
        private Perlin surfaceNoise1 = new Perlin();
        private Perlin surfaceNoise2 = new Perlin();
        private FastNoiseCave caveNoise1 = new FastNoiseCave();
        private FastNoiseCave caveNoise2 = new FastNoiseCave();
        
        public override float CreateVoxelData(Vector3Int position)
        {
            var x = position.x * scale;
            var y = position.y * scale;
            var z = position.z * scale;

            // Hills
            surfaceNoise1.Seed = seed;
            surfaceNoise2.Seed = seed/2;
            
            float hills = (((float)surfaceNoise1.GetValue(x, surfaceBeginLevel, z) * hillScale) -((position.y - surfaceBeginLevel) * hillCutoffScale));
            hills += (((float) surfaceNoise2.GetValue(x, surfaceBeginLevel, z) * secondaryHillScale) -((position.y - surfaceBeginLevel) * secondaryHillNoiseScale));
            hills = Math.Min(Math.Max(hills, -1.0f), 1.0f);

            // Underground noise
            //value -= ((float)noise.GetValue(x, y, z) * noiseScale) + offset;

            // Caves
            caveNoise1.SetSeed(seed);
            caveNoise2.SetSeed(seed/2);
            
            caveNoise1.SetCellularJitter(0.3f);
            caveNoise2.SetCellularJitter(0.3f);
            
            caveNoise1.SetFrequency(caveFrequency);
            caveNoise2.SetFrequency(caveFrequency);
            
            caveNoise1.SetCellularReturnType(FastNoiseCave.CellularReturnType.Distance2Div);
            caveNoise2.SetCellularReturnType(FastNoiseCave.CellularReturnType.Distance2Div);
            
            float caveNoiseValue1 = caveNoise1.GetCellular(x, y, z);
            x += caveFrequency * 0.5f;
            y += caveFrequency * 0.5f;
            z += caveFrequency * 0.5f;
            float caveNoiseValue2 = caveNoise2.GetCellular(x, y, z);

            float caveNoiseValue = Math.Min(caveNoiseValue1, caveNoiseValue2);
            //if (caveNoiseValue < caveCutoffLevel)
                //return 0;

            caveNoiseValue = Math.Min(Math.Max(caveNoiseValue, -1.0f), 1.0f);
            float caves = (caveNoiseValue * caveNoiseScale);

            return hills - caves;

        }
    }
}