using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemy : MonoBehaviour
{
    public Transform center; // the center point of the circle
    public new Transform camera;
    public float damageRecived;
    public float radius = 29f;
    public GameObject prefab;
    public Transform playerTransform;

    private GameObject rifleInstanciated = null;
    
    Animator animator;

    private float x;
    private float z;
    private float y;

    private float acceleration = 1f; // acceleration factor
    public float currentSpeed = 1f;
    private float angle = 0f;
    private int orientation = -1;
    private float speedY = 0f;
    private float gravity = 0.6f;
    private float movementTimer;
    private float waitTimer = 0f;

    private Vector3 dist_player;

    private CharacterController characterController;
    private BoxCollider boxCol;

    //Stats
    private float shield = 25f;
    private float health = 50f;

    private float maxHealth = 50f;
    private float maxShield = 25f;

    private GameObject canvasLifeBar; // adapted lifeBar with interactions
    private GameObject canvasShieldBar;

    private UI_LifeBar scriptLifeBar;
    private UI_ShieldBar scriptShieldBar;

    // Start is called before the first frame update
    void Start()
    {
        angle = -2.3f;
        x = center.position.x + Mathf.Cos(angle) * radius;
        z = center.position.z + Mathf.Sin(angle) * radius;
        y = transform.position.y;
        transform.position = new Vector3(x, y, z);
        
        characterController = GetComponent<CharacterController>();
        boxCol = GetComponent<BoxCollider>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isMoving", true);
        //Physics.IgnoreCollision(characterController, player, true);
        
        //player = playerObject.GetComponent<characterController>();
        lifeBarCreation();
        shieldBarCreation();
        damageRecived = 0f;
        movementTimer = Random.Range(1f, 2f);
        getWeapon();
    }

    private void getWeapon()
    {
        rifleInstanciated = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(1).gameObject;
        RifleEnemy script = rifleInstanciated.GetComponent<RifleEnemy>();
        script.soundScript = gameObject.GetComponent<HumanEnemySound>();
        script.center = center;
        script.radius = radius;
        script.orientation = orientation;
    }

    private void lifeBarCreation()
    {
        canvasLifeBar = Instantiate(prefab, transform.position, Quaternion.identity);
        scriptLifeBar = canvasLifeBar.transform.GetComponent<UI_LifeBar>();
        scriptLifeBar.maxHealth = maxHealth;

        scriptLifeBar.actualHealth = health;
        scriptLifeBar.camera = camera;
        scriptLifeBar.orientation = orientation;
    }

    private void shieldBarCreation()
    {
        GameObject shieldBar = Resources.Load("prefabs/UI/Enemy/ShieldBar") as GameObject;
        Vector3 pos = transform.position + new Vector3(0f, 2f, 0f);
        canvasShieldBar = Instantiate(shieldBar, pos, Quaternion.identity);
        scriptShieldBar = canvasShieldBar.transform.GetComponent<UI_ShieldBar>();
        scriptShieldBar.maxHealth = maxShield;
        scriptShieldBar.actualHealth = shield;
        scriptShieldBar.camera = camera;
        scriptShieldBar.orientation = orientation;
    }

    private void controlDamage()
    {
        if (damageRecived != 0f)
        {
            if (shield > 0f)
            {
                shield -= damageRecived;
                if (!scriptShieldBar.Equals(null))
                    scriptShieldBar.actualHealth = shield;
            }
            else
            {
                health -= damageRecived;
                if (!scriptLifeBar.Equals(null))
                    scriptLifeBar.actualHealth = health;
            }
            damageRecived = 0;

            //Debug.Log("Enemy health: " + health.ToString());
            //Debug.Log("Enemy Shield: " + shield.ToString());
        }

        if (!scriptShieldBar.Equals(null) & shield <= 0f)
        {
            Destroy(canvasShieldBar);
            canvasShieldBar = null;
            health += shield;
            scriptLifeBar.actualHealth = health;
            shield = 0f;
        }

        if (health <= 0f)
        {
            Destroy(canvasLifeBar);
            Destroy(gameObject);
        }
    }

    void Friction()
    {
        if (Mathf.Abs(currentSpeed) <= 0.19f && Mathf.Abs(currentSpeed) >= 0.17f)
        {
            if (currentSpeed < 0f)
                currentSpeed = -0.2f;
            else
                currentSpeed = 0.2f;
        }
        else if (currentSpeed > 0.2f)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else if (currentSpeed < 0.2f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
    }

    private void controlAttack()
    {
        dist_player = transform.position - playerTransform.position;

        if (dist_player.magnitude < 10f && !rifleInstanciated.GetComponent<RifleEnemy>().canShoot)
        {
            Vector3 aux = Vector3.Normalize(dist_player);
            float dir_of_attack = Vector3.Angle(aux, transform.forward);
            Debug.Log(dir_of_attack);

            if (dir_of_attack > 60f)
            {
                orientation = -orientation;
                currentSpeed = -currentSpeed;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            rifleInstanciated.GetComponent<RifleEnemy>().canShoot = true;
            movementTimer = 0f;
        }
        else if(waitTimer == 0f)
        {
            rifleInstanciated.GetComponent<RifleEnemy>().canShoot = false;
        }
        /*else
        {
            //animator.SetBool("isAttack", false);
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controlDamage();

        controlAttack();

        float prevAngle = angle;

        // Adjust the angle based on the current speed
        if (movementTimer > 0f && waitTimer == 0f)
        {
            angle += currentSpeed / 2f * Time.deltaTime;
            angle %= (2 * Mathf.PI);
        }

        // Calculate the new position based on the angle and radius
        x = center.position.x + Mathf.Cos(angle) * radius;
        z = center.position.z + Mathf.Sin(angle) * radius;
        y = transform.position.y + speedY;

        if ((speedY < 0) && characterController.isGrounded)
            speedY = 0.0f;

        speedY -= gravity * Time.deltaTime;

        Friction();

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;
        Vector3 position = transform.position;

        if (movementTimer > 0f && waitTimer == 0f)
        {
            animator.SetBool("isMoving", true);
            CollisionFlags collition = characterController.Move(displace);

            if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
            {
                transform.position = new Vector3(position.x, transform.position.y, position.z);
                //Physics.SyncTransforms();
                angle = prevAngle;

                currentSpeed = -currentSpeed;
                orientation = -orientation;
            }
            
        }

        if (movementTimer == 0f)
        {
            animator.SetBool("isMoving", false);
            waitTimer = 2f;
            movementTimer = Random.Range(1f, 2f);
            if (dist_player.magnitude >= 13f)
            {
                int aux = Random.Range(0, 2);
                Debug.Log(aux);
                if (aux == 1)
                {
                    currentSpeed = -currentSpeed;
                    orientation = -orientation;
                }
            }
        }

        if (!scriptLifeBar.Equals(null))
        {
            scriptLifeBar.posEnemy = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            scriptLifeBar.orientation = orientation;
            scriptLifeBar.camera = camera;
        }   

        if (!scriptShieldBar.Equals(null))
        {
            scriptShieldBar.posEnemy = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            scriptShieldBar.orientation = orientation;
            scriptShieldBar.camera = camera;
        }


        if (waitTimer == 0f)
        {
            movementTimer -= Time.deltaTime;
            if (movementTimer < 0f)
                movementTimer = 0f;
        }

        waitTimer -= Time.deltaTime;
        if (waitTimer < 0f)
            waitTimer = 0f;

        rifleInstanciated.GetComponent<RifleEnemy>().orientation = orientation;
        rifleInstanciated.GetComponent<RifleEnemy>().angle = angle;
        Debug.Log("The orientation is " + orientation.ToString());
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
