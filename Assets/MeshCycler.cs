using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshCycler : MonoBehaviour
{
    public float fps = 8f;
    public Mesh[] meshes;

    MeshFilter meshFilter;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")) {
            int index = ((int)(Time.time * fps)) % meshes.Length;
            meshFilter.sharedMesh = meshes[index];
        }
        else
        {
            meshFilter.sharedMesh = meshes[0];
        }
    }
}
