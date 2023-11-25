using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public GameObject prefab;

    //damage recived;
    public bool isHurted;
    public bool isShoted;

    public float radius = 29f; // radius of the circle

    private CharacterController characterController;

    
    private float acceleration = 2f; // acceleration factor
    private float maxVelocity = 0.5f; // maximum rotation speed

    private float currentSpeed = 0f;
    private float angle = 0f;
    private float gravity = 0.6f;
    private float speedY = 0f;
    private int orientation = -1;
    private float input = 0f;

    private float x;
    private float z;
    private float y;

    public bool doJump = false;

    private float timer = 0f;


    //Stats
    private float health;


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
        health = 100f;
        isHurted = false;
        isShoted = false;
    }

    private void controlDamageImpact()
    {
        if (isHurted)
        {
            health -= 25f;
            isHurted = false;
            Debug.Log("Player health: " + health.ToString());
        }

        if (isShoted)
        {
            health -= 15f;
            isShoted = false;
            Debug.Log("Player health: " + health.ToString());
        }

        if (health <= 0f) {
            Destroy(gameObject);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
            
    }

    private void FixedUpdate()
    {
        controlDamageImpact();
        // Debug.Log(angle);
        // Adjust the current speed based on input and acceleration
        currentSpeed += input * acceleration * Time.deltaTime;

        // Clamp the speed to stay within the specified range
        currentSpeed = Mathf.Clamp(currentSpeed, -maxVelocity, maxVelocity);
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

        if (doJump)
        {
            speedY = 0.25f;
            doJump = false;
        }

        Friction(input);

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;
        Vector3 position = transform.position;
        CollisionFlags collition = characterController.Move(displace);
        if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
        {
            transform.position = new Vector3(position.x,transform.position.y,position.z);
            Physics.SyncTransforms();
            angle = prevAngle;
            currentSpeed = 0f;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

        
    }

    private void createBullet()
    {
        // Initialize values
        float bulletAngle = angle;
        bool leftMove;
        if (orientation == -1)
        {
            leftMove = false;
            bulletAngle += 0.05f;
        }
        else
        {
            leftMove = true;
            bulletAngle -= 0.05f;
        }

        //Compute position
        float xPos = center.position.x + Mathf.Cos(bulletAngle) * radius;
        float zPos = center.position.z + Mathf.Sin(bulletAngle) * radius;
        Vector3 pos = new Vector3(xPos, transform.position.y + 1f, zPos);

        //compute orientation (will be needed)

        //instantiate
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        //asign initiallization
        obj.GetComponent<Bullet>().leftMove = leftMove;
        obj.GetComponent<Bullet>().angle = bulletAngle;
        obj.GetComponent<Bullet>().radius = radius;

        //Destroy the object in 5 s
        //Destroy(obj, 7);

    }


    void Update()
    {
        input = 0f;
        float correction = Vector3.Angle((transform.position-center.position), transform.forward);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            input = 1f;
            if (orientation == 1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            orientation = -1;
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            input = -1f;
            if (orientation == -1)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            orientation = 1;
            transform.Rotate(0.0f, 90.0f - correction, 0.0f);
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && GetComponent<CharacterController>().isGrounded)
        {
            doJump = true;
        }

        if (Input.GetKey(KeyCode.P) & timer == 0f)
        {
            timer = 1f;
            createBullet();
           
        }
        
    }
}
