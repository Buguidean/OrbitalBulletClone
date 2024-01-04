using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public float angle;
    public int orientation;
    public float radius;
    public Transform center;
    public PlayerSounds soundScript;
    public bool changed = false;
    public bool isShoting = false;
    public bool canShoot = true;

    private float timer;
    
    public int ammo;

    private float shotRate = 1f;
    private float bulletTime = 0.875f;
    private float bulletDamage = 20f;

    private GameObject trailPrefab;

    private void Start()
    {
        trailPrefab = Resources.Load("prefabs/BulletTrailPlayer") as GameObject;
    }

    private void createBullet()
    {
        // Initialize values
        float bulletAngle = angle;
        bool leftMove;
        if (orientation == -1)
        {
            leftMove = false;
            bulletAngle += 0.06f * 29f / radius;
        }
        else
        {
            leftMove = true;
            bulletAngle -= 0.06f * 29f / radius;
        }

        //Compute position
        float xPos = center.position.x + Mathf.Cos(bulletAngle) * radius;
        float zPos = center.position.z + Mathf.Sin(bulletAngle) * radius;
        Vector3 pos = new Vector3(xPos, transform.position.y + 1.65f, zPos);

        //compute orientation (will be needed)

        //instantiate
        GameObject bulledPrefab = Resources.Load("prefabs/Sphere") as GameObject;
        GameObject obj = Instantiate(bulledPrefab, pos, Quaternion.identity);

        GameObject trail = Instantiate(trailPrefab, obj.transform.position, Quaternion.identity);
        trail.GetComponent<Follow>().bullet = obj.transform;
        Destroy(trail, 7f);

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
        if (changed)
        {
            changed = false;
            timer = shotRate;
        }

        if (canShoot && Input.GetKey(KeyCode.P) & timer == 0f & ammo > 0)
        {
            timer = shotRate;
            ammo -= 1;
            isShoting = true;
            soundScript.pistolSound = true;
            createBullet();
            Debug.Log(ammo);

        }
        else if (canShoot && ammo == 0 && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("No hay municion");
            soundScript.noAmmoSound = true;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

    }

}
