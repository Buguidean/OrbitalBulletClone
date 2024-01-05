using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoChest : MonoBehaviour
{
    public float damageRecived = 0f;
    public new Transform camera;

    private float health = 30f;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    void createAmmo()
    {
        Vector3 position_ammo = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        GameObject ammoPrefab = Resources.Load("prefabs/AmmoBox") as GameObject;
        GameObject obj = Instantiate(ammoPrefab, position_ammo, Quaternion.identity);
        obj.transform.rotation *= Quaternion.Euler(0, -20, 0);
        obj.GetComponent<CollectAmmo>().initialY = transform.position.y - 1.5f;
        obj.GetComponent<CollectAmmo>().camera = camera;
        obj.GetComponent<AmmoChestSound>().destroySound = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
            animator.SetBool("Hit", false);
        */
        //transform.rotation = camera.rotation;
        if (damageRecived != 0f)
        {
            health -= damageRecived;
            damageRecived = 0f;
            animator.Play("Hitted",0,0);
        }

        if (health <= 0f)
        {
            createAmmo();
            Destroy(gameObject);
        }
    }
}
