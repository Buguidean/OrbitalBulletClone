using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private float coolDown = 0f;
    private float initCoolDown = 0.7f;

    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag)
        {
            case "Player":
                //if(coolDown == 0f)
                //{
                obj.GetComponent<CircularMotion>().damageRecived = 10f;
                obj.GetComponent<CircularMotion>().doJump = true;
                //obj.GetComponent<PlayerSounds>().gruntSound = true;
                coolDown = initCoolDown;
                //}
                break;
        }
    }

    /*private void FixedUpdate()
    {
        coolDown -= Time.deltaTime;
        if (coolDown < 0f)
            coolDown = 0f;
    }*/
}
