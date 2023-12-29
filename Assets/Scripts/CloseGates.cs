using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGates : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.y < gameObject.transform.position.y)
        {
            animator.Play("open",0,0);
        }
    }
}
