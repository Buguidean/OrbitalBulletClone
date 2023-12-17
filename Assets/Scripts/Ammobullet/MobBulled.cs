using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBulled : MonoBehaviour
{ 
    public Transform player;
    public Transform center;

    public bool playerCollided;

    private Vector3 displacement;
    private Vector3 goal;
    private float maxTimer = 2f;
    private float timer;
    private bool shoted;
    private float speedY;
    private float gravity;

    private float damage = 25f;

    private void Start()
    {
        timer = maxTimer;
        displacement = new Vector3(0f, 0f, 0f);
        gravity = 0f;
        speedY = 0f;
    }

    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag)
        {
            case "Player":
                Debug.Log("The bullet impacted with player");
                obj.GetComponent<CircularMotion>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Environment":
                Debug.Log("BulledMob was distroyed because touched the environment");
                Destroy(gameObject);
                break;
            case "FlyingMob":
                break;
            default:
                Debug.Log("BulledMob was distroyed because touched something: " + obj.tag);
                Destroy(gameObject);
                break;
        }
    }

    private void outCylinder()
    {
        float xDist = Mathf.Abs(gameObject.transform.position.x - center.position.x);
        float zDist = Mathf.Abs(gameObject.transform.position.z - center.position.z);

        if (xDist > 30f | zDist > 30f)
        {
            Debug.Log("BulledMob out od cylinder");
            Destroy(gameObject);
        }

    }

    private void move()
    {
        Vector3 initialPos = gameObject.transform.position;
        gameObject.transform.position += new Vector3(-displacement.x/5f, speedY, -displacement.z/5f);

        speedY -= gravity * Time.deltaTime;
        

        if (speedY < -0.5f)
            speedY = -0.5f;

        Vector3 actualPos = gameObject.transform.position;
        if (actualPos.x < goal.x)
            actualPos.x = goal.x;
        if (actualPos.z < goal.z)
            actualPos.z = goal.z;
        if (actualPos.y < goal.y)
            actualPos.y = goal.y;
    }

    private void move2()
    {
        Vector3 initialPos = gameObject.transform.position;
        gameObject.transform.position -= displacement / 3f;

        Vector3 actualPos = gameObject.transform.position;
        if (actualPos.x < goal.x)
            actualPos.x = goal.x;
        if (actualPos.z < goal.z)
            actualPos.z = goal.z;
        if (actualPos.y < goal.y)
            actualPos.y = goal.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer == 0f & !shoted) //attack
        {
            shoted = true;
            gameObject.transform.SetParent(null);
            Vector3 middlePlayer = player.transform.position + new Vector3(0f, 1f, 0f);
            Vector3 direction = transform.position - middlePlayer;
            displacement = Vector3.Normalize(direction);
            goal = middlePlayer;
            Vector3 dist_player = player.position - transform.position;
            float maxDist = Mathf.Abs(Mathf.Max(dist_player.x, dist_player.y));
            Debug.Log("MaxDist: " + maxDist.ToString("0.000"));
            Debug.Log("displacement Y: " + displacement.y.ToString("0.000"));

            speedY = Mathf.Abs(maxDist)/80f;
            gravity = Mathf.Abs(displacement.y)*0.6f;

            Debug.Log("SpeedY: " + speedY.ToString("0.000"));
            Debug.Log("Gravity: " + gravity.ToString("0.000" ));
            


        }

        move();

        outCylinder();

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;
    }
}
