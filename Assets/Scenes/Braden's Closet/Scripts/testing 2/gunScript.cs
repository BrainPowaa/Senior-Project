using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    public ParticleSystem laser;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            var emi = laser.emission;
            emi.enabled = true;
        }
        else
        {
            var emi = laser.emission;
            emi.enabled = false;
        }
    }
}
