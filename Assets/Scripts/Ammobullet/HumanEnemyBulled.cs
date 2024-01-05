using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyBulled : MonoBehaviour
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
        explosion = Resources.Load("prefabs/Spheres Explode") as GameObject;
        rbController = GetComponent<Rigidbody>();
        if (leftMove)
        {
            currentSpeed = -2f * 14.5f / radius;
        }
        else
        {
            currentSpeed = 2f * 14.5f / radius;
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
                obj.GetComponent<CircularMotion>().damageRecived = damage;
                Destroy(gameObject);
                break;
            default:
                //aux.GetComponent<BulletSound>().impactSound = true;
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
        rbController.Move(newPosition, Quaternion.identity);

        timer -= Time.deltaTime * (14.5f / radius);

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
