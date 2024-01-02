using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBullet : MonoBehaviour
{
    public float damage;
    public GameObject explosion;

    //movement
    private float speedY;
    private float y;

    private void Start()
    {
        speedY = 0f;
    }

    void OnTriggerEnter(Collider obj)
    {
        GameObject aux = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(aux, 2f);
        switch (obj.tag)
        {
            case "Player":
                //Debug.Log("The bullet impacted with player");
                obj.GetComponent<CircularMotion>().damageRecived = damage;
                Destroy(gameObject);
                break;
            case "Bullet":
                break;
            case "BossBullet":
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void FixedUpdate()
    {
        // Calculate the new position based on the angle and radius
        y = transform.position.y + speedY;
        speedY -= 0.2f * Time.deltaTime;

        Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }
}
