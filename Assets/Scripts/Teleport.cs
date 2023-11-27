using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalRadius : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CircularMotion>().teleport = true;
            // The tag is the label that you put in an object manually (not the name)
            //Debug.Log("Collition with player");
            /*float rad = other.GetComponent<CircularMotion>().radius;
            switch (rad)
            {
                case (17f):
                    rad = 29f;
                    break;
                case (29f):
                    rad = 17f;
                    break;
            }
            other.GetComponent<CircularMotion>().radius = rad;
            */
        }
        //CircularMotion cm = other.GameObject.GetComponent<CircularMotion>();
    }

}