using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform center; // the center point of the circle
    private CharacterController characterController;

    private float radius = 29f; // radius of the circle

    private float currentSpeed = 1f;
    private float angle = 0f;

    private float x;
    private float z;
    private float y;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        characterController.Move(displace);
    }
}
