using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public bool leftMove;
    public float angle;
    public float radius; // radius of the circle

    private Rigidbody rbController;

    //movement

    private float currentSpeed; 

    private float x;
    private float z;
    private float y;

    private float timer = 7f;

    private void Start()
    {
        rbController = GetComponent<Rigidbody>();
        if (leftMove) {
            currentSpeed = -0.9f;
        }
        else {
            currentSpeed = 0.9f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Player":
                Debug.Log("The bullet was destroyed because touched Player");
                Destroy(gameObject);
                break;
            case "Enemy":
                Debug.Log("The bullet was destroyed because touched an Enemy");
                Destroy(gameObject);
                break;
            case "Bullet":
                Debug.Log("Bullet were touched");
                //Destroy(gameObject);
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

        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
