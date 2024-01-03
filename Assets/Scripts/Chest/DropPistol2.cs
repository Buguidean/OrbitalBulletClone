using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPistol2 : MonoBehaviour
{
    Animator animator;

    private float zPlayer;
    private Transform playerTransform;
    private GameObject pistol = null;
    private GameObject rifle = null;
    private float velocity = 0.9f;

    private float hasWeapon;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void dropPistol()
    {
        Vector3 pistolPos = transform.position;

        GameObject pistolPrefab = Resources.Load("prefabs/pistolDef") as GameObject;
        pistol = Instantiate(pistolPrefab, pistolPos + new Vector3(0f,-1f,0f), Quaternion.identity);

        pistol.transform.rotation = playerTransform.rotation;
        pistol.GetComponent<Pistol>().enabled = false;
    }

    private void dropRifle()
    {
        Vector3 pistolPos = transform.position;

        GameObject riflePrefab = Resources.Load("prefabs/rifleDef") as GameObject;
        rifle = Instantiate(riflePrefab, pistolPos + new Vector3(0f, -0.5f, 0f), Quaternion.identity);

        rifle.transform.rotation = playerTransform.rotation;
        rifle.GetComponent<Rifle>().enabled = false;
    }

    private void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Player")
        {
            if(!animator.GetBool("Open"))
                obj.GetComponent<CircularMotion>().showInteract = true;
            else
                obj.GetComponent<CircularMotion>().showInteract = false;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finish") && (pistol != null || rifle != null))
                obj.GetComponent<CircularMotion>().showInteract = true;
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            obj.GetComponent<CircularMotion>().showInteract = false;
        }
    }

    private void OnTriggerStay(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerTransform = obj.transform;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finish") && (pistol != null || rifle != null))
                    obj.GetComponent<CircularMotion>().showInteract = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                hasWeapon = obj.GetComponent<CircularMotion>().hasWeapon;
                
                if (!animator.GetBool("Open"))
                {
                    animator.SetBool("Open", true);
                    if (hasWeapon == 0)
                        dropPistol();
                    else
                        dropRifle();

                    obj.GetComponent<CircularMotion>().showInteract = false;
                    gameObject.GetComponent<WeaponChestSounds>().openSound = true;
                }

                if (hasWeapon == 0)
                {
                    if (pistol != null & animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                    {
                        obj.GetComponent<CircularMotion>().takePistol = true;
                        Destroy(pistol);
                        obj.GetComponent<CircularMotion>().showInteract = false;
                    }
                }
                else
                {
                    if (rifle != null & animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                    {
                        obj.GetComponent<CircularMotion>().takeRifle = true;
                        Destroy(rifle);
                        obj.GetComponent<CircularMotion>().showInteract = false;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Openning"))
        {
            if(hasWeapon == 0)
                pistol.transform.Translate(new Vector3(0f,velocity, 0f) * Time.deltaTime);
            else
                rifle.transform.Translate(new Vector3(0f, velocity, 0f) * Time.deltaTime);
        }

        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Openning"))
        {
            Debug.Log("Se esta abriendo");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
        {
            Debug.Log("Ha terminado");
        }*/
    }
}
