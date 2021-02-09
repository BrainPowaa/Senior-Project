using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshCycler : MonoBehaviour
{
    public float walkfps = 8f;
    public float idlefps = 4f;
    public Mesh[] meshes;

    MeshFilter meshFilter;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
            int index = ((int)(Time.time * walkfps)) % 4;
            meshFilter.sharedMesh = meshes[index];
        }
        else
        {
            int index = ((int)(Time.time * idlefps)) % 2;
            if (index != 0)
            {
                index = 4;
            }
            meshFilter.sharedMesh = meshes[index];
        }
    }
}
