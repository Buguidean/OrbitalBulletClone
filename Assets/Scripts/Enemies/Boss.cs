using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform center;
    public float startAngle;
    public float radius = 30f;
    public CharacterController player;
    public Transform playerTransform;

    public GameObject LifeBarObject;
    public float damageRecived;

    public GameObject ParentBoss;

    private UI_LifeBar_Boss scriptLifeBar;

    private float x;
    private float z;
    private float y;

    private float acceleration = 1f; // acceleration factor
    private float currentSpeed = 0.2f;
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
    private bool canMove = false;

    //attack
    private Transform leftParticle;
    private Transform rightParticle;
    private Transform reactor;
    private GameObject explosion;
    private GameObject flameAsset;
    private GameObject fireParticle;
    private GameObject bulletPrefab;
    private GameObject staticbulletPrefab;
    private GameObject trailPrefab;
    private int randomAttack;

    //timers
    private float coolDown = 0f;
    private float attackDuration = 0f;
    private float bulletDurationL = 0f;
    private float bulletDurationR = 0f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        angle = startAngle;
        x = center.position.x + Mathf.Cos(angle) * radius;
        z = center.position.z + Mathf.Sin(angle) * radius;
        y = transform.position.y;
        transform.position = new Vector3(x, y, z);

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        boxCol = GetComponent<BoxCollider>();
        Physics.IgnoreCollision(characterController, player, true);

        scriptLifeBar = LifeBarObject.transform.GetChild(0).gameObject.GetComponent<UI_LifeBar_Boss>();
        scriptLifeBar.actualHealth = health;
        scriptLifeBar.maxHealth = maxHealth;
        LifeBarObject.SetActive(false);
        damageRecived = 0f;

        getParticles();
        explosion = Resources.Load("prefabs/Spheres Explode") as GameObject;
        flameAsset = Resources.Load("prefabs/FlameThrower") as GameObject;
        bulletPrefab = Resources.Load("prefabs/BossEnemyBullet") as GameObject;
        staticbulletPrefab = Resources.Load("prefabs/BossEnemyStaticB") as GameObject;
        trailPrefab = Resources.Load("prefabs/BulletTrail") as GameObject;
    }

    private void getParticles()
    {
        leftParticle = gameObject.transform.GetChild(2).GetChild(1).GetChild(7);
        rightParticle = gameObject.transform.GetChild(3).GetChild(1).GetChild(7);
        reactor = gameObject.transform.GetChild(0).GetChild(0);
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
        if (t.childCount == 0 && t.name != "particleLeft" && t.name != "particleRight" && t.name != "center"
            && t.name != "m1" && t.name != "m2" && t.name != "m3" && t.name != "m4" && t.name != "m5"
            && t.name != "m6" && t.name != "m7" && t.name != "m8" && t.name != "m9" && t.name != "m10")
        {
            t.GetComponent<MeshRenderer>().material = m;
        }
        else
        {
            if (t.name != "cuboid")
            {
                for (int i = 0; i < t.childCount; i++)
                {
                    callChilds(t.GetChild(i), m);
                }
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
        }

        if (health <= 0f)
        {
            player.gameObject.GetComponent<PlayerMusic>().stopMusic = true;
            player.gameObject.GetComponent<CircularMotion>().invulnerable = true;
            Destroy(LifeBarObject);
            ParentBoss.GetComponent<MakeExplosion>().lastBossPosition = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
            Destroy(gameObject);
        }
    }

    private void controlAttack()
    {
        dist_player = transform.position - playerTransform.position;

        if (coolDown == 0f  && dist_player.magnitude < 47f)
        {
            
            randomAttack = Random.Range(0,2);
            if (randomAttack == 1)
            {
                attackDuration = 4f;
                coolDown = 7f;
            }
            else
            {
                attackDuration = 5.5f;
                coolDown = 8f;
            }

            if (attackDuration != 0f && randomAttack == 1)
            {
                if (!animator.GetBool("BigShoot"))
                    animator.Play("ReadyUp", 0, 0);
                animator.SetBool("BigShoot", true);
            }
            else if (attackDuration != 0f && randomAttack == 0)
            {
                if (!animator.GetBool("StaticShoot"))
                    animator.Play("ReadyUp", 0, 0);
                animator.SetBool("StaticShoot", true);
            }

            currentSpeed = 0f;
            
        }
        else if (attackDuration <= 0f)
        {
            attackDuration = 0f;
            currentSpeed = 0.2f;
            animator.SetBool("BigShoot", false);
            animator.SetBool("StaticShoot", false);
        }

        if (attackDuration != 0f && randomAttack == 1)
        {
            if (leftParticle.position.y > 53.9f)
            {
                GameObject aux1 = Instantiate(explosion, leftParticle.position, Quaternion.identity);
                if (bulletDurationL == 0f)
                {
                    bulletDurationL = 0.2f;
                    leftParticle.GetComponent<BossSound>().shotSound = true;
                }
                Destroy(aux1, 2f);

                /////////////////////////////////////////

                float randomAngle = Random.Range(0, 2 * Mathf.PI);
                Vector3 bullet_pos = new Vector3(center.position.x + Mathf.Cos(randomAngle) * radius, 100f, center.position.z + Mathf.Sin(randomAngle) * radius);

                GameObject obj = Instantiate(bulletPrefab, bullet_pos, Quaternion.identity);
                GameObject trail = Instantiate(trailPrefab, bullet_pos, Quaternion.identity);
                obj.GetComponent<BossEnemyBullet>().damage = 30f;
                obj.GetComponent<BossEnemyBullet>().explosion = explosion;
                trail.GetComponent<Follow>().bullet = obj.transform;
                Destroy(trail, 5f);

                /////////////////////////////////////////
            }
            if (rightParticle.position.y > 53.9f)
            {
                GameObject aux2 = Instantiate(explosion, rightParticle.position, Quaternion.identity);
                if (bulletDurationR == 0f)
                {
                    bulletDurationR = 0.2f;
                    rightParticle.GetComponent<BossSound>().shotSound = true;
                }
                Destroy(aux2, 2f);

                /////////////////////////////////////////

                float randomAngle = Random.Range(0, 2 * Mathf.PI);
                Vector3 bullet_pos = new Vector3(center.position.x + Mathf.Cos(randomAngle) * radius, 100f, center.position.z + Mathf.Sin(randomAngle) * radius);

                GameObject obj = Instantiate(bulletPrefab, bullet_pos, Quaternion.identity);
                GameObject trail = Instantiate(trailPrefab, bullet_pos, Quaternion.identity);
                obj.GetComponent<BossEnemyBullet>().damage = 30f;
                obj.GetComponent<BossEnemyBullet>().explosion = explosion;
                trail.GetComponent<Follow>().bullet = obj.transform;
                Destroy(trail, 5f);

                /////////////////////////////////////////
            }
        }

        else if (attackDuration != 0f && randomAttack == 0)
        {
            if (leftParticle.position.y < 52.4f && bulletDurationL == 0f)
            {
                bulletDurationL = 0.2f;

                GameObject aux1 = Instantiate(explosion, leftParticle.position, Quaternion.identity);
                leftParticle.GetComponent<BossSound>().shotSound = true;
                Destroy(aux1, 2f);

                /////////////////////////////////////////

                Vector3 lpos = new Vector3(leftParticle.position.x, leftParticle.position.y - 1.5f, leftParticle.position.z);

                GameObject obj = Instantiate(staticbulletPrefab, lpos, Quaternion.identity);
                GameObject trail = Instantiate(trailPrefab, lpos, Quaternion.identity);
                obj.GetComponent<BossEnemyStaticBullet>().leftMove = false;
                obj.GetComponent<BossEnemyStaticBullet>().angle = angle + 0.2f;
                obj.GetComponent<BossEnemyStaticBullet>().radius = radius;
                obj.GetComponent<BossEnemyStaticBullet>().damage = 20f;
                obj.GetComponent<BossEnemyStaticBullet>().timer = 3.5f;
                obj.GetComponent<BossEnemyStaticBullet>().center = center;
                obj.GetComponent<BossEnemyStaticBullet>().explosion = explosion;
                trail.GetComponent<Follow>().bullet = obj.transform;
                Destroy(trail, 7f);

                /////////////////////////////////////////
            }
            if (rightParticle.position.y < 52.4f && bulletDurationR == 0f)
            {
                bulletDurationR = 0.2f;

                GameObject aux2 = Instantiate(explosion, rightParticle.position, Quaternion.identity);
                rightParticle.GetComponent<BossSound>().shotSound = true;
                Destroy(aux2, 2f);

                /////////////////////////////////////////
                
                Vector3 rpos = new Vector3(rightParticle.position.x, rightParticle.position.y - 1.5f, rightParticle.position.z);

                GameObject obj = Instantiate(staticbulletPrefab, rpos, Quaternion.identity);
                GameObject trail = Instantiate(trailPrefab, rpos, Quaternion.identity);
                obj.GetComponent<BossEnemyStaticBullet>().leftMove = true;
                obj.GetComponent<BossEnemyStaticBullet>().angle = angle - 0.2f;
                obj.GetComponent<BossEnemyStaticBullet>().radius = radius;
                obj.GetComponent<BossEnemyStaticBullet>().damage = 20f;
                obj.GetComponent<BossEnemyStaticBullet>().timer = 3.5f;
                obj.GetComponent<BossEnemyStaticBullet>().center = center;
                obj.GetComponent<BossEnemyStaticBullet>().explosion = explosion;
                trail.GetComponent<Follow>().bullet = obj.transform;
                Destroy(trail, 7f);

                /////////////////////////////////////////
            }
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(leftParticle.position.y);
        //Debug.Log(rightParticle.position.y);

        //Debug.Log(gameObject.transform.position.y);
        //Debug.Log(player.transform.position.y);

        if (player.isGrounded && playerTransform.position.y + 3f >= gameObject.transform.position.y && !canMove)
        {
            LifeBarObject.SetActive(true);
            canMove = true;
            animator.Play("ReadyUp", 0, 0);
            player.gameObject.GetComponent<PlayerMusic>().isBossMusic = true;
        }

        controlDamage();

        if (damageTimer == 0f & materialSet)
        {
            materialSet = false;
            callChilds(gameObject.transform, Resources.Load("Materials/Boss") as Material);
        }

        float correction;
        if (canMove)
            correction = Vector3.Angle((transform.position - center.position), -transform.right);
        else
            correction = Vector3.Angle((transform.position - center.position), -transform.right);

        if (orientation == 1)
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
        else
            transform.Rotate(0.0f, 90.0f - correction, 0.0f);

        if (canMove)
        {
            if (fireParticle == null)
            {
                fireParticle = Instantiate(flameAsset, reactor.position, Quaternion.identity);
                fireParticle.transform.Translate(0f, 1f, 0f);
                fireParticle.transform.Rotate(0f, 0f, -90f);
                fireParticle.transform.SetParent(reactor);
            }

            if (characterController.isGrounded)
            {
                controlAttack();
            }

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

            //Friction();

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
        }

        if (!startTeleport && characterController.isGrounded && attackDuration == 0f)
        {
            dist_player = transform.position - playerTransform.position;
            randomNumber = Random.Range(0, 100);
            if (randomNumber == 1 && (dist_player.magnitude < 15 || dist_player.magnitude > 47))
            {
                startTeleport = true;
                GetComponent<BossSound>().jumpSound = true;
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

        coolDown -= Time.deltaTime;
        if (coolDown < 0f)
            coolDown = 0f;

        attackDuration -= Time.deltaTime;
        if (attackDuration < 0f)
            attackDuration = 0f;

        bulletDurationL -= Time.deltaTime;
        if (bulletDurationL < 0f)
            bulletDurationL = 0f;

        bulletDurationR -= Time.deltaTime;
        if (bulletDurationR < 0f)
            bulletDurationR = 0f;
    }
}
