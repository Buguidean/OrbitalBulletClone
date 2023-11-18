using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    private CharacterController characterController;

    private float radius = 29f; // radius of the circle
    private float acceleration = 8f; // acceleration factor
    private float maxVelocity = 0.6f; // maximum rotation speed

    private float currentSpeed = 0f;
    private float angle = 0f;
    private float gravity = 10.0f;
    private float speedY = 0f;
    private int orientation = 1;
    private float input = 0f;

    private float x;
    private float z;
    private float y;

    void Friction(float input)
    {
        if (input == 0f && currentSpeed > 0.0f)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else if (input == 0f && currentSpeed < 0.0f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
    }

    private void Start()
    {
        x = center.position.x + Mathf.Cos(0f) * radius;
        z = center.position.z + Mathf.Sin(0f) * radius;
        y = transform.position.y + speedY;
        characterController = GetComponent<CharacterController>();
        transform.position.Set(28.59f, 7.46f, -9.21f);
    }


    void Update()
    {
        input = 0f;
        float correction = Vector3.Angle((transform.position-center.position), transform.forward);

        // Check for input to accelerate or decelerate
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            input = -1f;
            if (orientation == 1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            orientation = -1;
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            input = 1f;
            if (orientation == -1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            orientation = 1;
            transform.Rotate(0.0f, 90.0f - correction, 0.0f);
        }

        // Adjust the current speed based on input and acceleration
        currentSpeed += input * acceleration * Time.deltaTime;

        // Clamp the speed to stay within the specified range
        currentSpeed = Mathf.Clamp(currentSpeed, -maxVelocity, maxVelocity);

        // Adjust the angle based on the current speed
        angle += currentSpeed * Time.deltaTime;
        angle %= (2 * Mathf.PI);

        // Calculate the new position based on the angle and radius
        if (currentSpeed != 0f || speedY != 0f)
        {
            x = center.position.x + Mathf.Cos(angle) * radius;
            z = center.position.z + Mathf.Sin(angle) * radius;
            y = transform.position.y + speedY;
        }

        if ((speedY < 0) && characterController.isGrounded)
            speedY = 0.0f;

        speedY -= gravity * Time.deltaTime;


        Friction(input);

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;

        if (currentSpeed != 0f || speedY != 0f)
            characterController.Move(displace);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((characterController.collisionFlags & CollisionFlags.Sides) != 0)
            currentSpeed = 0f;
    }
}
