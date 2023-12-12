using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPistol : MonoBehaviour
{

    private void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Player")
        {
            obj.GetComponent<CircularMotion>().takePistol = true;
            Destroy(gameObject);
        }
    }

}
