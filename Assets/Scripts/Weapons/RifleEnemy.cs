using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : MonoBehaviour
{
    public float angle;
    public int orientation;
    public float radius;
    public Transform center;

    public HumanEnemySound soundScript;
    public bool isShoting = false;
    public bool canShoot = false;
    

    private float timer;
    private float shotRate = 0.6f;
    private float bulletDamage = 12f;
    private float bulletTime = 3.5f;


    private void createBullet()
    {
        // Initialize values
        float bulletAngle = angle;
        bool leftMove;
        if (orientation == -1)
        {
            leftMove = false;
            bulletAngle += 0.07f * 29f / radius;
        }
        else
        {
            leftMove = true;
            bulletAngle -= 0.07f * 29f / radius;
        }

        //Compute position
        float xPos = center.position.x + Mathf.Cos(bulletAngle) * 29f;
        float zPos = center.position.z + Mathf.Sin(bulletAngle) * 29f;
        Vector3 pos = new Vector3(xPos, transform.position.y + 1.2f, zPos);

        //instantiate
        GameObject bulledPrefab = Resources.Load("prefabs/Sphere") as GameObject;
        GameObject obj = Instantiate(bulledPrefab, pos, Quaternion.identity);

        //asign initiallization
        obj.GetComponent<Bullet>().leftMove = leftMove;
        obj.GetComponent<Bullet>().angle = bulletAngle;
        obj.GetComponent<Bullet>().radius = radius;
        obj.GetComponent<Bullet>().damage = bulletDamage;
        obj.GetComponent<Bullet>().timer = bulletTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canShoot & timer == 0f)
        {
            timer = shotRate;
            createBullet();
            isShoting = true;
            soundScript.shootSound = true;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;
    }


}
