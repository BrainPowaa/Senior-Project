using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public MonoBehaviour weaponScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, cam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
            PickUp();

        if (equipped)
        {
            transform.parent = player;

        }

        if (equipped && Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    private void PickUp()
    {
        transform.position = gunContainer.position;
        transform.rotation = gunContainer.rotation;
        equipped = true;
        slotFull = true;
        rb.isKinematic = true;
        coll.isTrigger = true;
        weaponScript.enabled = true;

    }
    private void Drop()
    {
        equipped = false;
        slotFull = false;
        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;
        weaponScript.enabled = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random));
        
    }


    void Start()
    {
        weaponScript.enabled = false;
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
            weaponScript.enabled = false;
        }
        if(equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            weaponScript.enabled = true;
        }
        
    }
}
