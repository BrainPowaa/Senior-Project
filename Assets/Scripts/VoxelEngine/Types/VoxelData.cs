using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public struct VoxelData
    {
        // Packed byte info for this voxel
        // first 4 bits are for intensity value
        // last 4 bits are for material index
        public byte info;

        public VoxelData(byte info) => (this.info) = (info);

        public VoxelData(byte intensity, byte materialIndex)
        {
            info = Byte.MinValue;
            
            info = PackData(intensity, materialIndex);
        }

        // Packs intensity and material index (nibbles) into a byte for storage
        public byte PackData(byte intensity, byte materialIndex)
        {
            intensity = (byte) (intensity << 4);
            materialIndex = (byte) (materialIndex & 0x0F);

            return (byte) (intensity | materialIndex);
        }
        
        // Packs intensity and material index (nibbles) into a byte for storage
        public (byte, byte) UnpackData()
        {
            byte intensity = (byte) (info >> 4);
            byte materialIndex = (byte) (info & 0x0F);

            return (intensity, materialIndex);
        }
    }
}