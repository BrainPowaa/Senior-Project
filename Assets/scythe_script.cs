using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scythe_script : MonoBehaviour
{
    private bool toggleActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            toggleActive = !toggleActive;
            transform.GetChild(1).gameObject.SetActive(toggleActive);
        }
    }
}
