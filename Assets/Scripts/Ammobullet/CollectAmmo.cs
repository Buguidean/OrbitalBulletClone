using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAmmo : MonoBehaviour
{
    public new Transform camera;
    public float initialY;

    private float speedY = 0.2f;
    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag)
        {
            case "Player":
                Debug.Log("The player took the ammo");
                obj.GetComponent<CircularMotion>().collectAmmo = true;
                Destroy(gameObject);
                break;
        }
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + speedY, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speedY -= 0.6f * Time.deltaTime;

        if (transform.position.y > initialY)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y + speedY,transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x,initialY,transform.position.z);
        }

        transform.rotation = camera.rotation;
    }
}
