using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Player")
        {
            Debug.Log("El player colisiona con el cofre de armas");
        }
    }
        
}
