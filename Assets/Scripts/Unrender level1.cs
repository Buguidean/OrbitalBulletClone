using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unrenderlevel1 : MonoBehaviour
{
    private float timer = 3f;

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
