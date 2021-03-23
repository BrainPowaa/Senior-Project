using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Orb_Script : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            var target = collision.gameObject;
            StartCoroutine(heal(target));
            


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator heal(GameObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.green;
        target.GetComponent<HealthBarScript>().Healthbar.fillAmount += .25f;
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        yield return new WaitForSeconds(1);
        target.GetComponent<Renderer>().material.color = Color.white;
        Destroy(gameObject);


    }
}
