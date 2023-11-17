using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public float radius =4.5f; // radius of the circle
    public float rotationSpeed = 2f; // base rotation speed
    public float acceleration = 2f; // acceleration factor
    public float maxVelocity = 5f; // maximum rotation speed

    private float currentSpeed = 0f;
    private float angle = 0f;
    private float gravity = 10.0f;
    private float speedY = 0f;
    
    void Friction(float input)
    {
        if (input == 0.0f && currentSpeed >= 0.0f)
        {
            currentSpeed -= acceleration * 0.4f * Time.deltaTime;
        }
        else if (input == 0.0f && currentSpeed < 0.0f)
        {
            currentSpeed += acceleration * 0.4f * Time.deltaTime;
        }
    }

    void Update()
    {
        // Check for input to accelerate or decelerate
        float horizontalInput = Input.GetAxis("Horizontal");

        // Adjust the current speed based on input and acceleration
        currentSpeed += horizontalInput * acceleration * Time.deltaTime;

        // Clamp the speed to stay within the specified range
        currentSpeed = Mathf.Clamp(currentSpeed, -maxVelocity, maxVelocity);

        // Adjust the angle based on the current speed
        angle += currentSpeed * Time.deltaTime;

        // Calculate the new position based on the angle and radius
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float z = center.position.z + Mathf.Sin(angle) * radius;

        if ((speedY < 0) && GetComponent<CharacterController>().isGrounded)
            speedY = 0.0f;

        speedY -= gravity * Time.deltaTime;

        Friction(horizontalInput);

        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        GetComponent<CharacterController>().Move(newPosition - transform.position);
        GetComponent<CharacterController>().Move(new Vector3(0f,speedY,0f));
    }
}
