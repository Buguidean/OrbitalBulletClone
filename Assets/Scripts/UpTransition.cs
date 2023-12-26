using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTransition : MonoBehaviour
{
    void OnTriggerEnter(Collider obj)
    {
        switch (obj.tag)
        {
            case "Player":
                obj.GetComponent<CircularMotion>().jumpTransition = true;
                obj.GetComponent<PlayerSounds>().transitionSound = true;
                break;
        }
    }
}
