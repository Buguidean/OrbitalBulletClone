using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAmmo : MonoBehaviour
{
    public new Transform camera;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = camera.rotation;
    }
}
