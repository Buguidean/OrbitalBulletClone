using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public bool leftMove;
    public float angle;
    public float radius; // radius of the circle

    public float timer;
    public float damage;

    private Rigidbody rbController;
    private GameObject explosion;

    //movement

    private float currentSpeed; 

    private float x;
    private float z;
    private float y;    

    private void Start()
    {
        explosion = Resources.Load("prefabs/Spheres Explode Player") as GameObject;
        rbController = GetComponent<Rigidbody>();
        if (leftMove) {
            currentSpeed = -2f * 14.5f / radius;
        }
        else {
            currentSpeed = 2f * 14.5f / radius;
        }
    }

    private void OnTriggerStay(Collider obj)
    {
        switch (obj.tag) 
        {
            case "Spider":
                GameObject aux1 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux1, 2f);
                Debug.Log("The bullet impacted with the spider");
                obj.GetComponent<BasicEnemyMovement>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "FlyingMob":
                GameObject aux2 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux2, 2f);
                Debug.Log("The bullet impacted with the mob");
                obj.GetComponent<FlyingEnemy>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "HumanEnemy":
                GameObject aux3 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux3, 2f);
                Debug.Log("The bullet impacted with the HumanEnemy");
                obj.GetComponent<HumanEnemy>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "ammoChest":
                GameObject aux4 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux4, 2f);
                Debug.Log("The bullet impacted with the ammo chest");
                obj.GetComponent<ammoChest>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Boss":
                GameObject aux5 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux5, 2f);
                obj.GetComponent<Boss>().damageRecived = damage;
                Debug.Log("The bullet impacted with the boss");
                Destroy(gameObject);
                break;
            case "HumanEnemyBullet":
                break;
            case "ammo":
                break;
            case "Bullet":
                break;
            case "WeaponChest":
                break;
            case "BossBullet":
                break;
            default:
                GameObject aux6 = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(aux6, 2f);
                Destroy(gameObject);
                break;
        }
        
    }

    void OnTriggerEnter(Collider obj)
    {
        GameObject aux = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(aux, 2f);
        switch (obj.tag)
        {
            case "Player":
                //Debug.Log("The bullet impacted with player");
                //obj.GetComponent<CircularMotion>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Spider":
                Debug.Log("The bullet impacted with the spider");
                aux.GetComponent<BulletSound>().impactSound = true;
                obj.GetComponent<BasicEnemyMovement>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "FlyingMob":
                Debug.Log("The bullet impacted with the mob");
                aux.GetComponent<BulletSound>().impactSound = true;
                obj.GetComponent<FlyingEnemy>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "HumanEnemy":
                Debug.Log("The bullet impacted with the HumanEnemy");
                aux.GetComponent<BulletSound>().impactSound = true;
                obj.GetComponent<HumanEnemy>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "HumanEnemyBullet":
                //aux.GetComponent<BulletSound>().impactSound = true;
                Debug.Log("The bullet impacted with the HumanEnemyBullet");
                //Destroy(gameObject);
                break;
            case "Bullet":
                Debug.Log("Bullet were touched");
                break;
            case "ammoChest":
                Debug.Log("The bullet impacted with the ammo chest");
                aux.GetComponent<BulletSound>().impactSound = true;
                obj.GetComponent<ammoChest>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Untagged":
                //Debug.Log("The bullet was destroyed because touched Untagged object");
                Destroy(gameObject);
                break;
            case "Environment":
                //Debug.Log("The bullet was destroyed because touched the Environment");
                Destroy(gameObject);
                break;
            case "Boss":
                aux.GetComponent<BulletSound>().impactSound = true;
                obj.GetComponent<Boss>().damageRecived = damage;
                Debug.Log("The bullet impacted with the boss");
                Destroy(gameObject);
                break;
            case "BossBullet":
                break;
            case "ammo":
                break;
            default:
                Destroy(aux);
                break;
        }
    }

    private void FixedUpdate()
    {
        // Adjust the angle based on the current speed
        angle += currentSpeed * Time.deltaTime;
        angle %= (2 * Mathf.PI);

        // Calculate the new position based on the angle and radius
        x = center.position.x + Mathf.Cos(angle) * radius;
        z = center.position.z + Mathf.Sin(angle) * radius;

        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        Vector3 displace = newPosition - transform.position;
        rbController.Move(newPosition, Quaternion.identity);

        timer -= Time.deltaTime * (14.5f/radius);

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
