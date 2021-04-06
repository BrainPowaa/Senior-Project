using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlashParameters
{
    public GameObject slash;
    public float delay;
}

public class SwordAttackScript : MonoBehaviour
{
    public bool updatePosition;
    public bool updateRotation;
    public List<SlashParameters> slashes;
    public Transform sword;

    private Animator anim;
    private SlashParameters effectToSpawn;
    private int currentAttack;

    void Start()
    {
        VFXSelecter(1);

        anim = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            VFXSelecter(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            VFXSelecter(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            VFXSelecter(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            VFXSelecter(4);
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack0" + currentAttack.ToString());
            StartCoroutine(Slash());
        }
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(effectToSpawn.delay);
        GameObject vfx = Instantiate(effectToSpawn.slash, sword.position, sword.rotation) as GameObject;
        Destroy(vfx, 2);

        if (updateRotation || updatePosition) {
            while(vfx != null)
            {
                if (updatePosition)
                    vfx.transform.position = sword.position;
                if (updateRotation)
                    vfx.transform.rotation = sword.rotation;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void VFXSelecter(int number)
    {
        currentAttack = number;
        if (slashes.Count > number - 1)
        {
            effectToSpawn = slashes[number - 1];
        }
        else
        {
            Debug.Log("Please assign a VFX in the inspector.");
        }
    }
}
