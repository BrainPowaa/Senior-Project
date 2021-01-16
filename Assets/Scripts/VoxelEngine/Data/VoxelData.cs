using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Data
{
    enum VoxelMaterial
    {
        Dirt,
        Rock,
        Sand,
        Grass
    }

    struct VoxelData
    {
        // Float representation of this point.
        float Intensity;

        // Material used by this point.
        VoxelMaterial Material;

        VoxelData(float intensity, VoxelMaterial material) => (Intensity, Material) = (intensity, material);
    }
}