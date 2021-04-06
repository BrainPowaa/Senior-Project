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

    private bool toggleActive = false;
    private SlashParameters effectToSpawn;
    private int currentAttack;

    void Start()
    {
        VFXSelecter(1);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            toggleActive = !toggleActive;
            transform.GetChild(1).gameObject.SetActive(toggleActive);
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Slash());
        }
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(effectToSpawn.delay);
        Quaternion targetRot = Quaternion.Euler(140, 180, 0);
        Vector3 targetScale = new Vector3(4, 4, 4);
        Vector3 targetPos = new Vector3(-3, 2, 3);
        GameObject vfx = Instantiate(effectToSpawn.slash, sword.position + targetPos, targetRot) as GameObject;
        vfx.transform.GetChild(0).transform.localScale += targetScale;
        vfx.transform.GetChild(0).GetChild(0).transform.localScale += targetScale;
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
