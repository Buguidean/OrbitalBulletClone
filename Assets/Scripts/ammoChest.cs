using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoChest : MonoBehaviour
{
    public bool isShoted = false;
    //public Transform camera;

    private float health = 20f;

    void createAmmo()
    {
        GameObject ammoPrefab = Resources.Load("prefabs/AmmoBox") as GameObject;
        GameObject obj = Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        obj.transform.rotation *= Quaternion.Euler(0, -20, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = camera.rotation;
        if (isShoted)
        {
            health -= 10f;
            isShoted = false;
        }

        if (health <= 0f)
        {
            createAmmo();
            Destroy(gameObject);
        }
    }
}
