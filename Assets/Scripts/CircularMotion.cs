using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public new Transform camera;

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

    public bool canMove = false;

    private GameObject pistolInstanciated = null;
    private GameObject rifleInstanciated = null;

    //has Weapon (0: any, 1: pistol, 2 rifle, 3 both (pistol active), 4 both (rifle active))
    public int hasWeapon;

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
    public bool dodging = false;
    public bool invulnerable = false;

    //Movement
    private bool constrained = false;

    private float acceleration = 2f; // acceleration factor
    private float maxVelocity = 0.5f; // maximum rotation speed

    public float currentSpeed = 0f;
    private float angle = 0f;
    private float gravity = 0.6f;
    private float speedY = 0f;
    public int orientation = 1;
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
    private GameObject LifeBar;

    //private bool isPistolUI = false;
    private GameObject pistolUI;
    private GameObject rifleUI;
    private GameObject pistolAmmoUI;
    private GameObject rifleAmmoUI;
    private GameObject backRifleUI;
    private GameObject backPistolUI;

    //Mano del player
    private Transform playerHand;
    private Transform playerHandMovement;
    private Transform playerHandMovement2;
    private Transform playerHandRight;

    //Sounds
    private PlayerSounds soundScript;

    //Dying
    private bool isDying = false;

    private float damageTimer;
    private bool materialSet = false;
   
    private float dodgingTimer = 0f;

    //interactions
    public bool showInteract = false;
    private GameObject interaction = null;

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

        getUI();
        pistolUI.SetActive(false);

        transform.rotation *= Quaternion.Euler(0, 180, 0);
        soundScript = gameObject.GetComponent<PlayerSounds>();
        hasWeapon = 0;
        createPistolHand();
        pistolInstanciated.SetActive(false);
        createRifleHand();
        rifleInstanciated.SetActive(false);
    }

    private void getUI()
    {
        pistolUI = camera.GetChild(0).GetChild(0).gameObject;
        rifleUI = camera.GetChild(0).GetChild(1).gameObject;
        pistolAmmoUI = pistolUI.transform.GetChild(0).gameObject;
        rifleAmmoUI = rifleUI.transform.GetChild(0).gameObject;
        backPistolUI = pistolUI.transform.GetChild(1).gameObject;
        backRifleUI = rifleUI.transform.GetChild(1).gameObject;

        pistolUI.SetActive(false);
        rifleUI.SetActive(false);

        pistolAmmoUI.GetComponent<TMP_Text>().text = pistolAmmo.ToString();
        rifleAmmoUI.GetComponent<TMP_Text>().text = rifleAmmo.ToString();

    }

    private void getHand()
    {
        playerHand = gameObject.transform.GetChild(36);//gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
        playerHandMovement = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2);//.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        playerHandMovement2 = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        playerHandRight = gameObject.transform.GetChild(40);
        //Debug.Log("El nom es:" + playerHandMovement.name);
    }

    private void createLifeBar()
    {
        GameObject lifeBar = Resources.Load("prefabs/UI/Player/LifeBar") as GameObject;
        Vector3 pos = camera.position + new Vector3(-2f, 1.55f, 3f);
        LifeBar = Instantiate(lifeBar, pos, Quaternion.identity);
        LifeBar.transform.SetParent(camera.transform);
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
            if (currentSpeed > 0.5f)
                currentSpeed -= acceleration * 2f * Time.deltaTime;
            else
                currentSpeed -= acceleration * Time.deltaTime;

        }
        else if (input == 0f && currentSpeed < 0f)
        {
            if (currentSpeed < 0.5f)
                currentSpeed += acceleration * 2f * Time.deltaTime;
            else
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
            if(health <= 0f)
            {
                //Prepare die animation
                timer = 1.5f;
                animator.SetBool("isMoving", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("hasRifle", false);
                rifleInstanciated.SetActive(false);
                pistolInstanciated.SetActive(false);
                if(orientation == 1)
                {
                    transform.Rotate(0, 180f, 0);
                }
                soundScript.dyingSound = true;
                isDying = true;
                animator.Play("death");
            }

            if (!isDying)
            {
                for (int i = 1; i < gameObject.transform.childCount - 3; i++)
                {
                    gameObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/PlayerDamaged") as Material;
                }
                soundScript.gruntSound = true;
                damageTimer = 0.1f;
                materialSet = true;
            }

            //Debug.Log("Player health: " + health.ToString());
        }

        /*if (health <= 0f)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }*/

    }

    private void showWeapon()
    {
        if (hasWeapon == 2 | hasWeapon == 4)
        {
            pistolInstanciated.SetActive(false);
            rifleInstanciated.SetActive(true);
            //animator.SetBool("hasRifle", true);
        }
        else if (hasWeapon == 1 | hasWeapon == 3)
        {
            rifleInstanciated.SetActive(false);
            pistolInstanciated.SetActive(true);
            animator.SetBool("hasRifle", false);
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
                        pistolInstanciated.GetComponent<Pistol>().ammo = maxAmmoPistol;
                        pistolAmmoUI.GetComponent<TMP_Text>().text = pistolAmmo.ToString();
                        break;
                    //Rifle
                    case 2:
                    case 4:
                        rifleAmmo = maxAmmoRifle;
                        rifleInstanciated.GetComponent<Rifle>().ammo = maxAmmoRifle;
                        rifleAmmoUI.GetComponent<TMP_Text>().text = rifleAmmo.ToString();
                        break;

                }
            }
            soundScript.collectAmmoSound = true;
            collectAmmo = false;
        }

        // Cambar segun el hasWeapon
        if (takeRifle)
        {
            switch (hasWeapon)
            {
                case 0:
                    animator.SetBool("hasWeapon", true);
                    rifleUI.SetActive(true);
                    hasWeapon = 2;
                    showWeapon();
                    break;
                case 1:
                    rifleUI.SetActive(true);
                    Image img = backPistolUI.GetComponent<Image>();
                    img.material = Resources.Load("Materials/Black") as Material;
                    hasWeapon = 4;
                    showWeapon();
                    break;

            }
            soundScript.takeWeaponSound = true;
            takeRifle = false;
        }

        if (takePistol)
        {
            switch (hasWeapon)
            {
                case 0:
                    animator.SetBool("hasWeapon", true);
                    pistolUI.SetActive(true);
                    hasWeapon = 1;
                    showWeapon();
                    break;
                case 2:
                    pistolUI.SetActive(true);
                    Image img = backRifleUI.GetComponent<Image>();
                    img.material = Resources.Load("Materials/Black") as Material;
                    hasWeapon = 3;
                    showWeapon();
                    break;
            }
            soundScript.takeWeaponSound = true;
            takePistol = false;
        }

    }

    private void FixedUpdate()
    {
        if (health <= 0f)
        {
            if ((speedY < 0) && characterController.isGrounded)
                speedY = 0.0f;

            speedY -= gravity * Time.deltaTime;

            y = transform.position.y + speedY;

            Vector3 newPosition = new Vector3(x, y, z);
            Vector3 displace = newPosition - transform.position;
            characterController.Move(displace);

            timer -= Time.deltaTime;

            if(timer <= 0f)
                SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else
        {
            //Debug.Log(this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            if (GetComponent<CharacterController>().isGrounded)//this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "jump" | 
            {
                if (animator.GetBool("isJumping"))
                {
                    animator.SetBool("isJumping", false);
                }
            }

            float correction = Vector3.Angle((transform.position - center.position), transform.forward);

            if (orientation == 1)
                transform.Rotate(0.0f, correction - 90.0f, 0.0f);
            else
                transform.Rotate(0.0f, 90.0f - correction, 0.0f);


            if (currentSpeed != 0f)
            {
                animator.SetBool("isMoving", true);
            }


            if (teleport && GetComponent<CharacterController>().isGrounded)
            {
                if (isExtRad)
                {
                    radius -= 0.25f;
                    speedY = 0.5f;
                }
                else
                {
                    radius += 0.25f;
                    speedY = 0.35f;
                }
                soundScript.teleport = true;
                

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

            // si la animaci�n no es la de esquivar, pon dodging a false y la velocidad a la que estava (currentSpeed /= 1.4f)
            if (this.animator.GetCurrentAnimatorClipInfoCount(0) > 0)
            {
                //Debug.Log(this.animator.GetCurrentAnimatorClipInfoCount(0));
                if (dodging && this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "roll")
                {
                    dodging = false;
                    maxVelocity = 0.5f;
                    dodgingTimer = 0.2f;
                }
            }


            if (!dodging & !invulnerable)
            {
                controlDamageImpact();
            }
            else
            {
                damageRecived = 0f;
            }

            if (damageTimer == 0f & materialSet)
            {
                materialSet = false;
                for (int i = 1; i < gameObject.transform.childCount - 3; i++)
                {
                    gameObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/Player") as Material;
                }
            }

            collectedObjects();

            // Adjust the current speed based on input and acceleration
            currentSpeed += input * acceleration * Time.deltaTime;

            // Clamp the speed to stay within the specified range
            currentSpeed = Mathf.Clamp(currentSpeed, -maxVelocity, maxVelocity);
            float prevAngle = angle;

            // Adjust the angle based on the current speed
            if (isExtRad)
            {
                angle += currentSpeed * Time.deltaTime;
                angle %= (2 * Mathf.PI);
            }
            else
            {
                angle += currentSpeed * 1.5f * Time.deltaTime;
                angle %= (2 * Mathf.PI);
            }

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
                animator.SetBool("isJumping", true);
            }

            if (doJumpHigh)
            {
                speedY = 0.25f;
                doJumpHigh = false;
                animator.Play("jump", 0, 0);
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

            damageTimer -= Time.deltaTime;
            if (damageTimer < 0f)
                damageTimer = 0f;

            dodgingTimer -= Time.deltaTime;
            if (dodgingTimer < 0f)
                dodgingTimer = 0f;

            //update attributes for bullet
            Pistol script1 = pistolInstanciated.GetComponent<Pistol>();
            script1.angle = angle;
            script1.orientation = orientation;
            script1.radius = radius;
            script1.canShoot = !dodging;

            Rifle script2 = rifleInstanciated.GetComponent<Rifle>();
            script2.angle = angle;
            script2.orientation = orientation;
            script2.radius = radius;
            script2.canShoot = !dodging;

            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
            if (showInteract && interaction == null)
            {
                GameObject prefab = Resources.Load("prefabs/interaction") as GameObject;
                interaction = Instantiate(prefab, pos, Quaternion.identity);
                interaction.transform.rotation = camera.rotation;
            }
            else if (!showInteract && interaction != null)
            {
                Destroy(interaction);
            }
            else if (showInteract && interaction != null)
            {
                interaction.transform.position = pos;
                interaction.transform.rotation = camera.rotation;
            }
        }
    }

    void swapUISelected()
    {
        Material selected = Resources.Load("Materials/WhiteSelected") as Material;
        Material black = Resources.Load("Materials/Black") as Material;
        if (hasWeapon == 4)
        {
            backRifleUI.GetComponent<Image>().material = selected;
            backPistolUI.GetComponent<Image>().material = black;
        }
        else
        {
            backRifleUI.GetComponent<Image>().material = black;
            backPistolUI.GetComponent<Image>().material = selected;
        }
        //pistolAmmoUI = pistolInstanciated.GetComponent<Pistol>.ammo;
        //rifleAmmoUI = pistolInstanciated.GetComponent<Rifle>.ammo;
    }

    private void createPistolHand()
    {
        float weaponAngle = angle;

        float addAngle = 0.04f;
        if (hasWeapon == 1 | hasWeapon == 3)
            addAngle = 0.05f;
        if (orientation == 1)
        {
            weaponAngle += addAngle * (29f / radius);
        }
        else
        {
            weaponAngle -= addAngle * (29f / radius);
        }
        float xPos = playerHand.transform.position.x - Mathf.Cos(weaponAngle) * 14.5f / radius * 1f;
        float zPos = playerHand.transform.position.z - Mathf.Cos(weaponAngle) * 14.5f / radius * 1f; //+ Mathf.Sin(weaponAngle) * radius;
        float yPos = playerHand.transform.position.y + 0.1f;
        pistolInstanciated = Instantiate(Resources.Load("prefabs/pistolDef") as GameObject, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        pistolInstanciated.transform.SetParent(playerHandMovement);

        Pistol script1 = pistolInstanciated.GetComponent<Pistol>();
        script1.angle = weaponAngle;
        script1.orientation = orientation;
        script1.radius = radius;
        script1.center = center;
        script1.ammo = pistolAmmo;
        script1.changed = false;
        script1.soundScript = gameObject.GetComponent<PlayerSounds>();

    }

    private void createRifleHand()
    {
        float weaponAngle = angle;

        float addAngle = 0.06f;
        if (orientation == 1)
        {
            weaponAngle += addAngle * (29f / radius);
        }
        else
        {
            weaponAngle -= addAngle * (29f / radius);
        }
        float xPos = playerHandRight.transform.position.x - (Mathf.Cos(weaponAngle) * 14.5f / radius) * 1f;
        float zPos = playerHandRight.transform.position.z - (Mathf.Cos(weaponAngle) * 14.5f / radius) * 0.7f;
        float yPos = playerHandRight.transform.position.y + 0.4f;
        rifleInstanciated = Instantiate(Resources.Load("prefabs/rifleDef") as GameObject, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        rifleInstanciated.transform.SetParent(playerHandMovement);
        Rifle script2 = rifleInstanciated.GetComponent<Rifle>();
        script2.angle = weaponAngle;
        script2.orientation = orientation;
        script2.radius = radius;
        script2.center = center;
        script2.ammo = rifleAmmo;
        script2.changed = false;
        rifleInstanciated.transform.rotation *= Quaternion.Euler(0, 17, 0);
        script2.soundScript = gameObject.GetComponent<PlayerSounds>();
    }

    void Update()
    { 
        if(health > 0f && canMove)
        {
            input = 0f;

            if (!constrained)
            {
                if (Time.deltaTime != 0f && Input.GetKey(KeyCode.D))
                {
                    input = 1f;
                    if (orientation == 1)
                        transform.Rotate(0.0f, 180.0f, 0.0f);
                    orientation = -1;
                }
                else if (Time.deltaTime != 0f && Input.GetKey(KeyCode.A))
                {
                    input = -1f;
                    if (orientation == -1)
                        transform.Rotate(0.0f, 180.0f, 0.0f);
                    orientation = 1;
                }

                if (Input.GetKeyDown(KeyCode.Space) && GetComponent<CharacterController>().isGrounded)
                {
                    doJump = true;
                    animator.SetBool("isJumping", true);

                    animator.Play("jump", 0, 0);

                    soundScript.jumpSound = true;
                }

                if (Input.GetKeyDown(KeyCode.O) && !dodging && dodgingTimer == 0f)
                {
                    maxVelocity = 0.7f;
                    dodging = true;
                    if (orientation == -1)
                        currentSpeed += 1f;
                    else
                        currentSpeed -= 1f;
                    animator.Play("roll", 0, 0);
                }

                //Key Cheats
                if (Input.GetKeyDown(KeyCode.M))
                {
                    pistolAmmo = maxAmmoPistol;
                    pistolInstanciated.GetComponent<Pistol>().ammo = maxAmmoPistol;
                    pistolAmmoUI.GetComponent<TMP_Text>().text = pistolAmmo.ToString();

                    rifleAmmo = maxAmmoRifle;
                    rifleInstanciated.GetComponent<Rifle>().ammo = maxAmmoRifle;
                    rifleAmmoUI.GetComponent<TMP_Text>().text = rifleAmmo.ToString();

                    soundScript.collectAmmoSound = true;
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    invulnerable = !invulnerable;

                    string s1 = "The player is ";
                    if (!invulnerable)
                        s1 += "not ";
                    Debug.Log(s1 + "invulnerable");
                }

                switch (hasWeapon)
                {
                    case 1:
                    case 3:
                        if (pistolAmmo > 0)
                        {
                            pistolAmmoUI.GetComponent<TMP_Text>().text = pistolInstanciated.GetComponent<Pistol>().ammo.ToString();
                        }
                        break;
                    case 2:
                    case 4:
                        if (rifleAmmo > 0)
                        {
                            rifleAmmoUI.GetComponent<TMP_Text>().text = rifleInstanciated.GetComponent<Rifle>().ammo.ToString();
                        }
                        break;
                }

                if (pistolInstanciated.GetComponent<Pistol>().isShoting)
                {
                    pistolInstanciated.GetComponent<Pistol>().isShoting = false;
                    animator.Play("pistol_shoot", 0, 0);
                    currentSpeed = 0f;
                }
                if (rifleInstanciated.GetComponent<Rifle>().isShoting)
                {
                    rifleInstanciated.GetComponent<Rifle>().isShoting = false;
                    animator.Play("rifle_fire", 0, 0);
                    currentSpeed = 0f;
                }

                timer -= Time.deltaTime;
                if (timer < 0f)
                    timer = 0f;

                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (hasWeapon == 3)
                    {
                        soundScript.changeWeaponSound = true;
                        hasWeapon = 4;
                        showWeapon();
                        rifleInstanciated.GetComponent<Rifle>().changed = true;
                        swapUISelected();
                    }
                    else if (hasWeapon == 4)
                    {
                        soundScript.changeWeaponSound = true;
                        hasWeapon = 3;
                        showWeapon();
                        pistolInstanciated.GetComponent<Pistol>().changed = true;
                        swapUISelected();
                    }
                }
            }
        }
    }
}