using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public CharacterController player;
    public float radius = 29f; // radius of the circle

    private CharacterController characterController;

    private float acceleration = 2f; // acceleration factor
    private float currentSpeed = 0.2f;
    private float angle = 0f;
    private float gravity = 0.5f;
    private float speedY = 0f;
    private int orientation = 1;

    private float x;
    private float z;
    private float y;

    //private bool doJump = false;

    //private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 vector = Vector3.Normalize(player.transform.position - characterController.transform.position);
            float angle_hit = Vector3.Angle(Vector3.up, vector);

            if (angle_hit <= 45f)
            {
                other.GetComponent<CircularMotion>().doJump = true;
                Destroy(gameObject);
            }
        }     
    }

    void Friction(float input)
    {
        if (input == 0f && Mathf.Abs(currentSpeed) <= 0.01f)
        {
            currentSpeed = 0f;
        }
        else if (input == 0f && currentSpeed > 0f)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else if (input == 0f && currentSpeed < 0f)
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
        Physics.IgnoreCollision(characterController, player, true);
        angle = -2.64f;
    }

    private void FixedUpdate()
    {
        // Adjust the current speed based on input and acceleration
        float prevAngle = angle;

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

        // Friction(input);

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;
        Vector3 position = transform.position;
        CollisionFlags collition = characterController.Move(displace);
        Debug.Log(collition);
        if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            Physics.SyncTransforms();
            angle = prevAngle;
            currentSpeed = -currentSpeed;
            orientation = -orientation;

            if (orientation == 1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            else if (orientation == -1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    


    void Update()
    {
        float correction = Vector3.Angle((transform.position - center.position), transform.forward);


        if (orientation == 1)
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);

        else if (orientation == -1)
            transform.Rotate(0.0f, 90.0f - correction, 0.0f);
            
    }
}
