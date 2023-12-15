using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public Transform camera;

    //damage recived    
    public float damageRecived;
    // orbit change 
    public bool teleport;

    public float radius = 29f; // radius of the     

    //weapons
    public bool collectAmmo = false;
    public bool takePistol = false;
    public bool takeRifle = false;

    public bool jumpTransition = false;

    public bool openedWC = false;

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
    private bool constrained = false;

    private float acceleration = 2f; // acceleration factor
    private float maxVelocity = 0.5f; // maximum rotation speed

    public float currentSpeed = 0f;
    private float angle = 0f;
    private float gravity = 0.6f;
    private float speedY = 0f;
    private int orientation = -1;
    private float input = 0f;

    private float x;
    private float z;
    private float y;

    public bool doJump = false;
    public bool doJumpHigh = false;

    private float timer = 0f;
    //private float shotTimer = 0f; 


    //Stats
    private float maxHealth;
    private float health;

    //UI
    GameObject LifeBar;

    //Mano del player
    private Transform playerHand;

    private void Start()
    {
        x = center.position.x + Mathf.Cos(0f) * radius;
        z = center.position.z + Mathf.Sin(0f) * radius;
        y = transform.position.y + speedY;
        characterController = GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        damageRecived = 0f;

        maxHealth = 100f;
        health = maxHealth;
        createLifeBar();
        getHand();
    }

    private void getHand()
    {
        playerHand = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
    }

    private void createLifeBar()
    {
        GameObject lifeBar = Resources.Load("prefabs/UI/Player/LifeBar") as GameObject;
        Vector3 pos = camera.position + new Vector3(-2f, 1.55f, 3f);
        LifeBar = Instantiate(lifeBar, pos, Quaternion.identity);
        LifeBar.transform.parent = camera.transform;
        UI_LifeBar_Player script = LifeBar.GetComponent<UI_LifeBar_Player>();
        script.maxHealth = maxHealth;
        script.actualHealth = health;
    }

    private void Friction(float input)
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

    private void controlDamageImpact()
    {
        if (damageRecived != 0f)
        {
            health -= damageRecived;
            damageRecived = 0f;
            LifeBar.GetComponent<UI_LifeBar_Player>().actualHealth = health;
            //Debug.Log("Player health: " + health.ToString());
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

        //Jump to the upper level
        if (jumpTransition && GetComponent<CharacterController>().isGrounded)
        {
            speedY = 0.8f;
            currentSpeed = 0f;
            jumpTransition = false;
            constrained = true;
        }

        if (constrained && speedY < 0f)
        {
            constrained = false;
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

        if (doJumpHigh)
        {
            speedY = 0.30f;
            doJumpHigh = false;
        }

        Friction(input);

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;
        Vector3 position = transform.position;
        CollisionFlags collition = characterController.Move(displace);
        if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            //Physics.SyncTransforms();
            angle = prevAngle;
            currentSpeed = 0f;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;

        //update attributes for bullet
        if (weaponInstanciated != null)
        {
            switch (hasWeapon)
            {
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
            weaponAngle += 0.04f * (29f / radius);
        }
        else
        {
            weaponAngle -= 0.04f * (29f / radius);
        }

        float xPos = center.position.x + Mathf.Cos(weaponAngle) * radius;
        float zPos = center.position.z + Mathf.Sin(weaponAngle) * radius;
        Vector3 pos = new Vector3(xPos, transform.position.y + 1f, zPos);

        GameObject weaponModel = Resources.Load("prefabs/p") as GameObject;
        switch (hasWeapon)
        {
            case 1:
            case 3:
                pos -= new Vector3(0f, 0.5f, 0f);
                break;
            case 2:
            case 4:
                weaponModel = Resources.Load("prefabs/model") as GameObject;
                break;
        }

        Vector3 zeros = new Vector3(0.2f, 0.5f, 0.1f);

        weaponInstanciated = Instantiate(weaponModel, pos, Quaternion.identity);
        weaponInstanciated.transform.parent = gameObject.transform;//playerHand;
        weaponInstanciated.transform.rotation = transform.rotation;
        switch (hasWeapon)
        {
            case 1:
            case 3:
                weaponInstanciated.transform.Rotate(0.0f, 180.0f, 0.0f);
                Pistol script1 = weaponInstanciated.GetComponent<Pistol>();
                script1.angle = weaponAngle;
                script1.orientation = orientation;
                script1.radius = radius;
                script1.center = center;
                script1.ammo = pistolAmmo;
                break;
            case 2:
            case 4:
                Rifle script2 = weaponInstanciated.GetComponent<Rifle>();
                script2.angle = weaponAngle;
                script2.orientation = orientation;
                script2.radius = radius;
                script2.center = center;
                script2.ammo = rifleAmmo;
                break;
        }

    }

    void Update()
    {
        //Debug
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log(angle);
        }

        input = 0f;
        float correction = Vector3.Angle((transform.position - center.position), transform.forward);

        if (!constrained)
        {
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

            if (Input.GetKey(KeyCode.C))
            {
                takePistol = true;
            }

            if (Input.GetKey(KeyCode.V))
            {
                takeRifle = true;
            }

            /*if (Input.GetKey(KeyCode.T) && hasWeapon != 0)
            {
                hasWeapon = 0;
                Destroy(weaponInstanciated);
                weaponInstanciated = null;
            }*/

            if (Input.GetKeyUp(KeyCode.E))
            {
                dodging = true;
                currentSpeed *= 1.4f;
                //change animation a roll
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
                if (hasWeapon == 3)
                {
                    hasWeapon = 4;
                    Destroy(weaponInstanciated);
                    weaponInstanciated = null;
                    createWeapon();
                }
                else if (hasWeapon == 4)
                {
                    hasWeapon = 3;
                    Destroy(weaponInstanciated);
                    weaponInstanciated = null;
                    createWeapon();
                }
            }
        }
    }
}