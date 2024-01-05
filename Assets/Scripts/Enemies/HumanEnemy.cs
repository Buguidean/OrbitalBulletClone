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
    public CharacterController player;

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

    private float damage = 25f;

    private GameObject canvasLifeBar; // adapted lifeBar with interactions
    private GameObject canvasShieldBar;
    private GameObject alert;
    private GameObject dieee;
    private GameObject ammo;

    private UI_LifeBar scriptLifeBar;
    private UI_ShieldBar scriptShieldBar;

    private float damageTimer;
    private bool materialSet = false;

    public float startAngle;

    // Start is called before the first frame update
    void Start()
    {
        angle = startAngle;
        x = center.position.x + Mathf.Cos(angle) * radius;
        z = center.position.z + Mathf.Sin(angle) * radius;
        y = transform.position.y;
        transform.position = new Vector3(x, y, z);

        characterController = GetComponent<CharacterController>();
        boxCol = GetComponent<BoxCollider>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isMoving", true);
        Physics.IgnoreCollision(characterController, player, true);

        //player = playerObject.GetComponent<characterController>();
        lifeBarCreation();
        shieldBarCreation();
        damageRecived = 0f;
        movementTimer = Random.Range(1f, 2f);
        getWeapon();
        alert = Resources.Load("prefabs/true_exclamation") as GameObject;
        dieee = Resources.Load("prefabs/Triangles Explode") as GameObject;
        ammo = Resources.Load("prefabs/AmmoBox") as GameObject;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CircularMotion script = other.GetComponent<CircularMotion>();
            script.damageRecived = damage;

            if (script.currentSpeed != 0f)
            {
                if (script.currentSpeed < 0f)
                    script.currentSpeed = 0.4f;
                else
                    script.currentSpeed = -0.4f;
            }
            else
            {
                if (script.orientation == -1)
                    script.currentSpeed = -0.4f;
                else
                    script.currentSpeed = 0.4f;
            }
        }
    }

    private void controlDamage()
    {
        if (damageRecived != 0f)
        {
            for (int i = 1; i < gameObject.transform.childCount - 3; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/EnemyDamaged") as Material;
            }
            damageTimer = 0.1f;
            materialSet = true;

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
            Vector3 pos_die_ammo = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.61f, gameObject.transform.position.z);
            Vector3 pos_die = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
            GameObject aux = Instantiate(dieee, pos_die, Quaternion.identity);
            GameObject aux2 = Instantiate(ammo, pos_die_ammo, Quaternion.identity);
            aux2.GetComponent<CollectAmmo>().initialY = pos_die_ammo.y;
            aux2.GetComponent<CollectAmmo>().camera = camera;
            aux.GetComponent<EemyExplodeSound>().explodeSound = true;
            Destroy(aux, 2f);
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
        bool sameHeight = playerTransform.position.y <= transform.position.y + 5f;

        if (sameHeight && dist_player.magnitude < 30f && !rifleInstanciated.GetComponent<RifleEnemy>().canShoot)
        {
            Vector3 aux = Vector3.Normalize(dist_player);
            float dir_of_attack = Vector3.Angle(aux, transform.forward);
            Debug.Log("Direction of attack: " + dir_of_attack.ToString() + " Orientation: " + orientation.ToString());

            if (dir_of_attack > 60f)
            {
                orientation = -orientation;
                currentSpeed = -currentSpeed;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }

            Vector3 alert_par_pos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            GameObject alert_par = Instantiate(alert, alert_par_pos, Quaternion.identity);
            alert_par.transform.rotation = camera.rotation;
            alert_par.transform.Rotate(0, 90, -90);
            Destroy(alert_par,0.5f);
            gameObject.GetComponent<HumanEnemySound>().alertSound = true;

            rifleInstanciated.GetComponent<RifleEnemy>().canShoot = true;
            rifleInstanciated.GetComponent<RifleEnemy>().animator = animator;
            movementTimer = 0f;
        }
        else if (waitTimer == 0f)
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
        if (player.transform.position.y >= transform.position.y - 3f)
        {
            controlDamage();

            controlAttack();

            if (damageTimer == 0f & materialSet)
            {
                materialSet = false;
                for (int i = 1; i < gameObject.transform.childCount - 3; i++)
                {
                    gameObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/HumanEnemy") as Material;
                }
            }



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
                movementTimer = Random.Range(1f, 3f);
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

            damageTimer -= Time.deltaTime;
            if (damageTimer < 0f)
                damageTimer = 0f;

            rifleInstanciated.GetComponent<RifleEnemy>().orientation = orientation;
            rifleInstanciated.GetComponent<RifleEnemy>().angle = angle;
            //Debug.Log("The orientation is " + orientation.ToString());
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
