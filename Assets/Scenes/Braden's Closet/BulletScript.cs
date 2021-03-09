﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Mob")
        {
            var target = collision.gameObject;
            target.GetComponent<HealthBarScript>().Fill -= .25f;

            gameObject.SetActive(false);
        }
    }


}