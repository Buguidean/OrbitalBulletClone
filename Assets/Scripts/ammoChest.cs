using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoChest : MonoBehaviour
{
    public bool isShoted = false;

    private float health = 20f;

    void createAmmo()
    {
        GameObject ammoPrefab = Resources.Load("prefabs/AmmoPrefab") as GameObject;
        GameObject obj = Instantiate(ammoPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
