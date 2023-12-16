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

    //movement

    private float currentSpeed; 

    private float x;
    private float z;
    private float y;    

    private void Start()
    {
        rbController = GetComponent<Rigidbody>();
        if (leftMove) {
            currentSpeed = -2f * 14.5f / radius;
        }
        else {
            currentSpeed = 2f * 14.5f / radius;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag)
        {
            case "Player":
                Debug.Log("The bullet impacted with player");
                obj.GetComponent<CircularMotion>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Spider":
                Debug.Log("The bullet impacted with the spider");
                obj.GetComponent<BasicEnemyMovement>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "FlyingMob":
                Debug.Log("The bullet impacted with the mob");
                obj.GetComponent<FlyingEnemy>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Bullet":
                Debug.Log("Bullet were touched");
                break;
            case "ammoChest":
                Debug.Log("The bullet impacted with the ammo chest");
                obj.GetComponent<ammoChest>().isShoted = true;
                Destroy(gameObject);
                break;
            case "Untagged":
                Debug.Log("The bullet was destroyed because touched the Environment");
                Destroy(gameObject);
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
