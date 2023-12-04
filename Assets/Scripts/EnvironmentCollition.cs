using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCollition : MonoBehaviour
{
    public GameObject player;

    private CharacterController characterController;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        characterController = player.GetComponent<CharacterController>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.transform.position.y < gameObject.transform.position.y)
            Physics.IgnoreCollision(characterController, boxCollider, true);
        else
            Physics.IgnoreCollision(characterController, boxCollider, false);
    }
}
