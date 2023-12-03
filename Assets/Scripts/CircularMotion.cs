using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public GameObject bulledPrefab;

    //damage recived    
    public float damageRecived;
    // orbit change 
    public bool teleport;

    public float radius = 29f; // radius of the     

    //weapons
    public GameObject pistol;
    public GameObject rifle;
    public bool collectAmmo = false;
    public bool takePistol = false;
    public bool takeRifle = false;
    

    private GameObject weaponInstanciated = null;
   

    //has Weapon (0: any, 1: pistol, 2 rifle, 3 both (pistol active), 4 both (rifle active))
    private int hasWeapon;

    private int maxAmmoPistol = 10;
    private int maxAmmoRifle = 30;

    private int pistolAmmo = 10;
    private int rifleAmmo = 30;

    private CharacterController characterController;

    Animator animator;

    //teleport control
    private float internalRadius = 14.5f;
    private float externalRadius = 29.0f;
    private bool isExtRad = true;

    //dodge attributes
    private bool dodging = false;
    private bool invulnerable = false;

    //Movement
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
    //private float shotTimer = 0f; 


    //Stats
    private float health;


    void Friction(float input)
    {
        if (input == 0f && Mathf.Abs(currentSpeed) <= 0.1f)
        {
            currentSpeed = 0.000000000000000000000000000000000000f;
            animator.SetBool("isMoving", false);
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
        animator = gameObject.GetComponent<Animator>();
        health = 100f;
        damageRecived = 0f;
    }

    private void controlDamageImpact()
    {
        if (damageRecived != 0f)
        {
            health -= damageRecived;
            damageRecived = 0f;
            Debug.Log("Player health: " + health.ToString());
        }

        if (health <= 0f)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

    }

    private void collectedObjects()
    {

        if (collectAmmo) // cambiar con la gestion de municion
        {
            if (hasWeapon > 0)
            {
                switch (hasWeapon)
                {
                    //pistol
                    case 1:
                    case 3:
                        pistolAmmo = maxAmmoPistol;
                        weaponInstanciated.GetComponent<Pistol>().ammo = maxAmmoPistol;
                        break;
                    //Rifle
                    case 2: 
                    case 4:
                        rifleAmmo = maxAmmoRifle;
                        weaponInstanciated.GetComponent<Rifle>().ammo = maxAmmoRifle;
                        break;

                }
            }

            collectAmmo = false;
        }
        // Cambar segun el hasWeapon
        if (takeRifle)
        {
            switch (hasWeapon)
            {
                case 0:
                    hasWeapon = 2;
                    createWeapon();
                    break;
                case 1:
                    hasWeapon = 4;
                    Destroy(weaponInstanciated);
                    weaponInstanciated = null;
                    createWeapon();
                    break;
            }
            takeRifle = false;
        }

        if (takePistol)
        {
            switch (hasWeapon)
            {
                case 0:
                    hasWeapon = 1;
                    createWeapon();
                    break;
                case 2:
                    hasWeapon = 3;
                    Destroy(weaponInstanciated);
                    weaponInstanciated = null;
                    createWeapon();
                    break;
            }
            takePistol = false;
        }
        
    }

    private void FixedUpdate()
    {

        if (currentSpeed != 0f)
        {
            animator.SetBool("isMoving", true);
        }
        
        if (teleport && GetComponent<CharacterController>().isGrounded)
        {
            if (isExtRad)
                radius -= 0.25f;
            else
                radius += 0.25f;

            speedY = 0.5f;
        }

        else if (teleport)
        {
            if (isExtRad)
            {
                if (radius <= internalRadius)
                {
                    teleport = false;
                    radius = internalRadius;
                    isExtRad = false;
                }
                else
                    radius -= 0.25f;
            }

            else
            {
                if (radius >= externalRadius)
                {
                    teleport = false;
                    radius = externalRadius;
                    isExtRad = true;
                }
                else
                    radius += 0.25f;

            }
        }

        // si la animación no es la de esquivar, pon dodging a false y la velocidad a la que estava (currentSpeed /= 1.4f)
        if (!dodging & !invulnerable)
        {
            controlDamageImpact();
        }
        else
        {
            damageRecived = 0f;
        }

        collectedObjects();

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
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            Physics.SyncTransforms();
            angle = prevAngle;
            currentSpeed = 0f;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

        //update attributes for bullet
        if (weaponInstanciated != null)
        {
            switch (hasWeapon) {
                case 1:
                case 3:
                    Pistol script1 = weaponInstanciated.GetComponent<Pistol>();
                    script1.angle = angle;
                    script1.orientation = orientation;
                    script1.radius = radius;
                    break;
                case 2:
                case 4:
                    Rifle script2 = weaponInstanciated.GetComponent<Rifle>();
                    script2.angle = angle;
                    script2.orientation = orientation;
                    script2.radius = radius;
                    break;
            }
        }


    }

    void createWeapon()
    {
        float weaponAngle = angle;

        if (orientation == -1)
        {
            weaponAngle += 0.04f * (29f/radius);
        }
        else
        {
            weaponAngle -= 0.04f * (29f/radius);
        }

        float xPos = center.position.x + Mathf.Cos(weaponAngle) * radius;
        float zPos = center.position.z + Mathf.Sin(weaponAngle) * radius;
        Vector3 pos = new Vector3(xPos, transform.position.y + 1f, zPos);
        
        GameObject weaponModel = pistol;
        switch (hasWeapon)
        {
            case 1:
            case 3:
                pos += new Vector3(0f, 0.5f, 0f);
                break;
            case 2:
            case 4:
                weaponModel = rifle;
                break;
        }
        
        weaponInstanciated = Instantiate(weaponModel, pos, Quaternion.identity);
        weaponInstanciated.transform.parent = gameObject.transform;
        weaponInstanciated.transform.rotation = transform.rotation;
        switch (hasWeapon)
        {
            case 1:
            case 3:
                weaponInstanciated.transform.Rotate(0.0f, 90.0f, 0.0f);
                Pistol script1 = weaponInstanciated.GetComponent<Pistol>();
                script1.angle = weaponAngle;
                script1.orientation = orientation;
                script1.radius = radius;
                script1.bulledPrefab = bulledPrefab;
                script1.center = center;
                script1.ammo = pistolAmmo;
                break;
            case 2:
            case 4:
                Rifle script2 = weaponInstanciated.GetComponent<Rifle>();
                script2.angle = weaponAngle;
                script2.orientation = orientation;
                script2.radius = radius;
                script2.bulledPrefab = bulledPrefab;
                script2.center = center;
                script2.ammo = rifleAmmo;
                break;
        }

    }

    void Update()
    {
        input = 0f;
        float correction = Vector3.Angle((transform.position - center.position), transform.forward);

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

        if (Input.GetKey(KeyCode.C) && hasWeapon == 0)
        {
            hasWeapon = 1;
            pistolAmmo = maxAmmoPistol;
            createWeapon();
        }

        if (Input.GetKey(KeyCode.V) && hasWeapon == 0)
        {
            hasWeapon = 2;
            createWeapon();
        }

        /*if (Input.GetKey(KeyCode.T) && hasWeapon != 0)
        {
            hasWeapon = 0;
            Destroy(weaponInstanciated);
            weaponInstanciated = null;
        }*/

        if (Input.GetKey(KeyCode.E))
        {
            dodging = true;
            currentSpeed *= 1.4f;
            //change animationa roll
        }

        //Key Cheats
        if (Input.GetKey(KeyCode.M))
        {
            collectAmmo = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            invulnerable = !invulnerable;

            string s1 = "The player is ";
            if (!invulnerable)
                s1 += "not ";
            Debug.Log(s1 + "invulnerable");
        }
        //

        if (Input.GetKey(KeyCode.P) & timer == 0f)
        {
            switch (hasWeapon)
            {
                case 1:
                case 3:
                    if (pistolAmmo > 0)
                    {
                        timer = 1f;
                        pistolAmmo -= 1;
                    }
                    break;
                case 2:
                case 4:
                    if (pistolAmmo > 0)
                    {
                        timer = 0.6f;
                        rifleAmmo -= 1;
                    }
                    break;
            }

        }
        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(hasWeapon == 3)
            {
                hasWeapon = 4;
                Destroy(weaponInstanciated);
                weaponInstanciated = null;
                createWeapon();
            }
            else if(hasWeapon == 4)
            {
                hasWeapon = 3;
                Destroy(weaponInstanciated);
            weaponInstanciated = null;
                createWeapon();
            }
        }
    }
    

}