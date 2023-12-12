using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPistol2 : MonoBehaviour
{
    Animator animator;


    private float zPlayer;
    private Transform playerTransform;
    private GameObject pistol = null;
    private float velocity = 0.9f;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void dropPistol()
    {
        Vector3 pistolPos = transform.position;

        GameObject pistolPrefab = Resources.Load("prefabs/dropPistol") as GameObject;
        pistol = Instantiate(pistolPrefab, pistolPos, Quaternion.identity);

        pistol.transform.rotation = playerTransform.rotation;
    }

    private void OnTriggerStay(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerTransform = obj.transform;

            if (Input.GetKeyUp(KeyCode.I))
            {
                if (!animator.GetBool("Open"))
                {
                    animator.SetBool("Open", true);
                    dropPistol();
                }

                if (pistol != null & animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                {
                    obj.GetComponent<CircularMotion>().takePistol = true;
                    Destroy(pistol);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Openning"))
        {
            
            pistol.transform.Translate(new Vector3(0f,velocity, 0f) * Time.deltaTime);
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
