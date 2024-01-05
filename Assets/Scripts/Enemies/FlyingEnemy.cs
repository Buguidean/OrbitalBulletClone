using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    public Transform center; // the center point of the circle
    public CharacterController player;
    public Transform playerTransform;
    public float radius = 29f; // radius of the circle
    public float modifier;
    public float dist_attack;
    public new Transform camera;

    public float damageRecived;

    private CharacterController characterController;
    private BoxCollider boxCol;
    private CircularMotion playerScript;
    Animator animator;
    private GameObject dieee;

    private float acceleration = 1f; // acceleration factor
    private float currentSpeed = 0.2f;
    private float angle = 0f;
    //private float gravity = 0.5f;
    private float speedY = 0f;
    private int orientation = -1;
    private Vector3 dist_player;

    private float x;
    private float z;
    private float y;

    //stats
    private float health = 50f;
    private float damage = 25f;
    private float shield = 25f;

    //Bar stats things
    public GameObject prefab; // prefab obj

    private GameObject canvasLifeBar; // adapted lifeBar with interactions
    private GameObject canvasShieldBar;
    private GameObject trailPrefab;
    private GameObject alert;

    private UI_LifeBar scriptLifeBar;
    private UI_ShieldBar scriptShieldBar;

    private float maxHealth = 50f;
    private float maxShield = 25f;

    private float damageTimer;
    private bool materialSet = false;

    public float startAngle;



    //attack
    private GameObject instanciatedBulled = null;

    //private bool doJump = false

    private float coolDown = 0f;

    private void Start()
    {
        x = center.position.x + Mathf.Cos(0f) * radius;
        z = center.position.z + Mathf.Sin(0f) * radius;
        y = transform.position.y + speedY;
        characterController = GetComponent<CharacterController>();
        boxCol = GetComponent<BoxCollider>();
        animator = gameObject.GetComponent<Animator>();
        Physics.IgnoreCollision(characterController, player, true);
        angle = startAngle;
        //player = playerObject.GetComponent<characterController>();
        lifeBarCreation();
        shieldBarCreation();
        damageRecived = 0f;
        instanciatedBulled = null;
        trailPrefab = Resources.Load("prefabs/BulletTrail") as GameObject;
        alert = Resources.Load("prefabs/true_exclamation_spider") as GameObject;
        dieee = Resources.Load("prefabs/Triangles Explode") as GameObject;
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

    private void callChilds(Transform t, Material m)
    {
        if (t.childCount == 0 && t.name != "BulledMob(Clone)" & t.name != "pyramid")
            t.GetComponent<MeshRenderer>().material = m;
        else
        {
            for (int i = 0; i < t.childCount; i++)
            {
                callChilds(t.GetChild(i), m);
            }
        }
    }

    private void controlDamage()
    {
        if (damageRecived != 0f)
        {
            callChilds(gameObject.transform, Resources.Load("Materials/EnemyDamaged") as Material);

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
            Vector3 pos_die = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
            GameObject aux = Instantiate(dieee, pos_die, Quaternion.identity);
            aux.GetComponent<EemyExplodeSound>().explodeSound = true;
            Destroy(aux, 2f);
            Destroy(canvasLifeBar);
            Destroy(gameObject);
        }
    }

    private void prepareAttack()
    {
        dist_player = playerTransform.position - transform.position;
        if (dist_player.magnitude < dist_attack & coolDown == 0f)
        {
            Debug.Log(dist_player.magnitude);
            GameObject bulledPrefab = Resources.Load("prefabs/BulledMob") as GameObject;
            Vector3 pos = gameObject.transform.position + new Vector3(0f, 1f, 0f);
            instanciatedBulled = Instantiate(bulledPrefab, pos, Quaternion.identity);
            GameObject trail = Instantiate(trailPrefab, pos, Quaternion.identity);
            trail.GetComponent<Follow>().bullet = instanciatedBulled.transform;
            Destroy(trail, 5f);
            
            instanciatedBulled.transform.parent = gameObject.transform;
            MobBulled script = instanciatedBulled.GetComponent<MobBulled>();
            script.player = playerTransform;
            script.center = center;
            if(dist_player.magnitude < 18)
            {
                script.modifier = 1f;
            }
            else
                script.modifier = modifier;
            coolDown = 2f;

            Vector3 alert_par_pos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            GameObject alert_par = Instantiate(alert, alert_par_pos, Quaternion.identity);
            alert_par.transform.rotation = camera.rotation;
            alert_par.GetComponent<Follow_spider>().spider = gameObject.transform;
            alert_par.transform.Rotate(0, 90, -90);
            Destroy(alert_par, 0.5f);
            gameObject.GetComponent<FlyingMobSound>().alertSound = true;
        }
    }

    private void FixedUpdate()
    {
        if (instanciatedBulled == null)
            prepareAttack();

        coolDown -= Time.deltaTime;
        if (coolDown < 0f)
            coolDown = 0f;

        controlDamage();

        if (damageTimer == 0f & materialSet)
        {
            materialSet = false;
            callChilds(gameObject.transform, Resources.Load("Materials/FlyingMob") as Material);
        }

        // Adjust the current speed based on input and acceleration
        float prevAngle = angle;

        // Adjust the angle based on the current speed
        angle += currentSpeed / 2f * Time.deltaTime;
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

        //speedY -= gravity * Time.deltaTime;

        Friction();

        Vector3 newPosition = new Vector3(x, y, z);
        Vector3 displace = newPosition - transform.position;
        displace.y = 0f;
        Vector3 position = transform.position;
        CollisionFlags collition = characterController.Move(displace);

        if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            //Physics.SyncTransforms();
            angle = prevAngle;
            currentSpeed = -currentSpeed;
            orientation = -orientation;
        }
        /*if (!scriptLifeBar.Equals(null))
        {*/
        scriptLifeBar.posEnemy = transform.position;
        scriptLifeBar.orientation = orientation;
        scriptLifeBar.camera = camera;
        //}   

        if (!scriptShieldBar.Equals(null))
        {
            scriptShieldBar.posEnemy = transform.position;
            scriptShieldBar.orientation = orientation;
            scriptShieldBar.camera = camera;
        }

        gameObject.transform.rotation = camera.rotation;
        gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0);

        damageTimer -= Time.deltaTime;
        if (damageTimer < 0f)
            damageTimer = 0f;
    }
}