using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Types
{
    public enum VoxelMaterial
    {
        Dirt,
        Rock,
        Sand,
        Grass
    }

    public struct VoxelData
    {
        // Float representation of this point.
        public float intensity;

        // Material used by this point.
        public VoxelMaterial material;

        public VoxelData(float intensity, VoxelMaterial material) => (this.intensity, this.material) = (intensity, material);
    }
}