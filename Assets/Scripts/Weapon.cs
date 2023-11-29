using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float angle;
    public int orientation;
    public float radius;
    public Transform center;

    public GameObject bulledPrefab;

    //public GameObject model;
    public int ammo;


    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void createBullet()
    {
        // Initialize values
        float bulletAngle = angle;
        bool leftMove;
        if (orientation == -1)
        {
            leftMove = false;
            bulletAngle += 0.05f * 29f/radius;
        }
        else
        {
            leftMove = true;
            bulletAngle -= 0.05f * 29f/radius;
        }

        //Compute position
        float xPos = center.position.x + Mathf.Cos(bulletAngle) * 29f;
        float zPos = center.position.z + Mathf.Sin(bulletAngle) * 29f;
        Vector3 pos = new Vector3(xPos, transform.position.y + 0.5f, zPos);

        //compute orientation (will be needed)

        //instantiate
        GameObject obj = Instantiate(bulledPrefab, pos, Quaternion.identity);

        //asign initiallization
        obj.GetComponent<Bullet>().leftMove = leftMove;
        obj.GetComponent<Bullet>().angle = bulletAngle;
        obj.GetComponent<Bullet>().radius = radius;

        //Destroy the object in 5 s
        //Destroy(obj, 7);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.P) & timer == 0f & ammo > 0)
        {
            timer = 1f;
            ammo -= 1;
            createBullet();
            Debug.Log(ammo);

        }
        else if (ammo == 0 & Input.GetKey(KeyCode.P))
        {
            Debug.Log("No hay municion");
        }
        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

        if (Input.GetKeyUp(KeyCode.S))
        {
            //Debug.Log("angle" + angle.ToString);
        }
    }


}
