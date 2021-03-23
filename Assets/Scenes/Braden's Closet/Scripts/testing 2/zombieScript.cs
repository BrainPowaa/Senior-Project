using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieScript : MonoBehaviour
{

    public float mobRange;
    public float mobSpeed = 3.0f;
    public Rigidbody mobRig;
    private GameObject wayPoint;
    private Vector3 wayPointPos;
    private bool found = false;
    private bool alive = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            var target = collision.gameObject;
            mobRig.AddForce(Vector3.forward * 10, ForceMode.Impulse);
            StartCoroutine(damage(target));
            
            
        }
    }

    void Start()
    {
        //At the start of the game, the zombies will find the gameobject called wayPoint.
        wayPoint = GameObject.Find("Player");
    }

    void Update()
    {
        
        float distance = Vector3.Distance(transform.position, wayPoint.transform.position);

        if (distance < mobRange && gameObject.GetComponent<HealthBarScript>().Healthbar.fillAmount > 0)
        {
            if (found == false)
            {
                mobRig.AddForce(Vector3.up * 3, ForceMode.Impulse);
                found = true;
            }
            transform.LookAt(wayPoint.transform);
            wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, wayPointPos, mobSpeed * Time.deltaTime);
        }

        if (gameObject.GetComponent<HealthBarScript>().Healthbar.fillAmount <= 0 && alive == true)
        {
            mobRig.AddForce(Vector3.forward * 10, ForceMode.Impulse);
            alive = false;
            StartCoroutine(die());
            
        }
    }
    IEnumerator die()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    IEnumerator damage(GameObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.red;
        target.GetComponent<HealthBarScript>().Healthbar.fillAmount -= .25f;
        target.GetComponent<Rigidbody>().AddForce(Vector3.back * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        target.GetComponent<Renderer>().material.color = Color.white;
    }
}