using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoChest : MonoBehaviour
{
    public bool isShoted = false;
    public new Transform camera;

    private float health = 30f;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    void createAmmo()
    {
        GameObject ammoPrefab = Resources.Load("prefabs/AmmoBox") as GameObject;
        GameObject obj = Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        obj.transform.rotation *= Quaternion.Euler(0, -20, 0);
        obj.GetComponent<CollectAmmo>().camera = camera;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
            animator.SetBool("Hit", false);
        */
        //transform.rotation = camera.rotation;
        if (isShoted)
        {
            health -= 10f;
            isShoted = false;
            animator.Play("Hitted",0,0);
        }

        if (health <= 0f)
        {
            createAmmo();
            Destroy(gameObject);
        }
    }
}
