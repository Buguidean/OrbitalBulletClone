using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : MonoBehaviour
{
    public float angle;
    public int orientation;
    public float radius;
    public Transform center;
    public Animator animator; 

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
        GameObject bulledPrefab = Resources.Load("prefabs/HumanEnemyBulled") as GameObject;
        GameObject trailPrefab = Resources.Load("prefabs/BulletTrail") as GameObject;
        GameObject obj = Instantiate(bulledPrefab, pos, Quaternion.identity);
        GameObject trail = Instantiate(trailPrefab, pos, Quaternion.identity);
        trail.GetComponent<Follow>().bullet = obj.transform;
        Destroy(trail, 5f);

        //asign initiallization
        obj.GetComponent<HumanEnemyBulled>().leftMove = leftMove;
        obj.GetComponent<HumanEnemyBulled>().angle = bulletAngle;
        obj.GetComponent<HumanEnemyBulled>().radius = radius;
        obj.GetComponent<HumanEnemyBulled>().damage = bulletDamage;
        obj.GetComponent<HumanEnemyBulled>().timer = bulletTime;
        obj.GetComponent<HumanEnemyBulled>().center = center;
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
            animator.Play("rifle_fire", 0, 0);
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;
    }


}
