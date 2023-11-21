using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public bool leftMove;
    public float angle;

    private Rigidbody rbController;

    private float radius = 29f; // radius of the circle

    private float currentSpeed; 

    private float x;
    private float z;
    private float y;

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
        Debug.Log("Triggered");
        //Destroy(rbController);
        //Destroy(this);

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
    }
}
