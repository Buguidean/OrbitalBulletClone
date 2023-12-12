using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponChest : MonoBehaviour
{
    Animator animator;

    private float zPlayer;
    private Transform playerTransform;
    private GameObject pistol = null;
    private bool dropped = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void dropPistol()
    {
        Vector3 pistolPos = transform.position;
        pistolPos.y += 1f;

        GameObject pistolPrefab = Resources.Load("prefabs/dropPistol") as GameObject;
        pistol = Instantiate(pistolPrefab, pistolPos, Quaternion.identity);

        pistol.transform.rotation = playerTransform.rotation;
    }

    private void OnTriggerStay(Collider obj)
    {
        if(obj.tag == "Player")
        {
            playerTransform = obj.transform;

            if (Input.GetKeyUp(KeyCode.I) )
            {
                if (!animator.GetBool("Open"))
                {
                    Debug.Log("Entro a abrir el cofre");
                    animator.SetBool("Open", true);
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                {
                    if(pistol != null)
                    {
                        obj.GetComponent<CircularMotion>().takePistol = true;
                        Destroy(pistol);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dropped & animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
        {
            dropPistol();
            dropped = true;
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
