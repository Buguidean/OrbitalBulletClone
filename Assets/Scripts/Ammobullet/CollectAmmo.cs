using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAmmo : MonoBehaviour
{
    public new Transform camera;
    public CharacterController characterController;

    private float speedY = 0.2f;
    private float y;
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
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((speedY < 0) && characterController.isGrounded)
            speedY = 0.0f;

        speedY -= 0.6f * Time.deltaTime;

        y = transform.position.y + speedY;

        Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
        Vector3 displace = newPosition - transform.position;
        characterController.Move(displace);
        transform.rotation = camera.rotation;
    }
}
