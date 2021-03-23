using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public float bulletSpeed = 1100;
    public GameObject bullet;

    AudioSource bulletAudio;

    // Use this for initialization
    void Start()
    {

        bulletAudio = GetComponent<AudioSource>();

    }

    void Fire()
    {
        GameObject tempBullet = Instantiate(bullet, transform.GetChild(2).position, transform.GetChild(2).rotation) as GameObject;
        Rigidbody tempRigidBodyBullet = tempBullet.GetComponent<Rigidbody>();
        tempRigidBodyBullet.AddForce(tempRigidBodyBullet.transform.forward * bulletSpeed);
        Destroy(tempBullet, 0.5f);

        bulletAudio.Play();

    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

    }
}
