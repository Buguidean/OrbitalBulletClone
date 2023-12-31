using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform center;
    public float radius = 29f;
    public CharacterController player;
    public Transform playerTransform;

    public GameObject LifeBarObject;
    public float damageRecived;

    private UI_LifeBar_Boss scriptLifeBar;

    private float x;
    private float z;
    private float y;

    private float acceleration = 1f; // acceleration factor
    public float currentSpeed = 1f;
    private float angle = 0f;
    private int orientation = -1;
    private float speedY = 0f;
    private float gravity = 0.6f;

    private CharacterController characterController;
    private BoxCollider boxCol;

    private Vector3 dist_player;

    //Stats
    private float health = 250f;
    private float maxHealth = 250f;

    private float damage = 25f;

    //teleport
    private bool startTeleport = false;
    private bool centerPass = false;
    private int randomNumber;
    private float radiusIncrement = -0.5f;

    private float damageTimer;
    private bool materialSet = false;

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
        Physics.IgnoreCollision(characterController, player, true);

        scriptLifeBar = LifeBarObject.transform.GetChild(0).gameObject.GetComponent<UI_LifeBar_Boss>();
        scriptLifeBar.actualHealth = health;
        scriptLifeBar.maxHealth = maxHealth;
        LifeBarObject.SetActive(false);
        damageRecived = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CircularMotion script = other.GetComponent<CircularMotion>();
            script.damageRecived = damage;
            other.GetComponent<PlayerSounds>().gruntSound = true;

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

    private void teleport()
    {
        if (radius == 0f)
        {
            centerPass = true;
            radiusIncrement = -radiusIncrement;
            angle += Mathf.PI;
        }

        if(centerPass && radius == 29f)
        {
            startTeleport = false;
            centerPass = false;
            radiusIncrement = -radiusIncrement;
        }

        radius += radiusIncrement;
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

            health -= damageRecived;
            if (!scriptLifeBar.Equals(null))
                scriptLifeBar.actualHealth = health;
            else
                Debug.Log("scriptLifeBar is Null");

            damageRecived = 0f;

            //Debug.Log("Enemy health: " + health.ToString());
            //Debug.Log("Enemy Shield: " + shield.ToString());
        }

        if (health <= 0f)
        {
            Destroy(LifeBarObject);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (player.isGrounded && playerTransform.position.y + 3f >= gameObject.transform.position.y)
        {
            LifeBarObject.SetActive(true);
        }
            

        controlDamage();

        if (damageTimer == 0f & materialSet)
        {
            materialSet = false;
            callChilds(gameObject.transform, Resources.Load("Materials/Boss") as Material);
        }

        float correction = Vector3.Angle((transform.position - center.position), -transform.right);

        if (orientation == 1)
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
        else
            transform.Rotate(0.0f, 90.0f - correction, 0.0f);

        float prevAngle = angle;

        // Adjust the angle based on the current speed
        angle += currentSpeed / 2f * Time.deltaTime;
        angle %= (2 * Mathf.PI);
        
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

        CollisionFlags collition = characterController.Move(displace);

        if (collition != CollisionFlags.None & collition != CollisionFlags.Below & collition != CollisionFlags.Above)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            //Physics.SyncTransforms();
            angle = prevAngle;

            currentSpeed = -currentSpeed;
            orientation = -orientation;
        }
        if (!startTeleport && characterController.isGrounded)
        {
            dist_player = transform.position - playerTransform.position;
            randomNumber = Random.Range(0, 4);
            if (randomNumber == 1 && dist_player.magnitude < 15)
            {
                startTeleport = true;
                speedY = 0.67f;

            }
        }
        else if (startTeleport)
        {
            teleport();
        }

        damageTimer -= Time.deltaTime;
        if (damageTimer < 0f)
            damageTimer = 0f;

    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
