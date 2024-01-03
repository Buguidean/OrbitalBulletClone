using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unrenderlevel1 : MonoBehaviour
{
    public Transform playerAnimate;

    private CircularMotion script;
    private float timer;

    void Start()
    {
        script = playerAnimate.gameObject.GetComponent<CircularMotion>();
        timer = 2f;
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            script.canMove = true;
            Destroy(gameObject);
        }
    }
}
