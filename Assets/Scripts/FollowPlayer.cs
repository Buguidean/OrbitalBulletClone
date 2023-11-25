using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform center;

    private Vector3 radius;
    private float offset_y = 0f;

    void Start()
    {
        radius = Vector3.Normalize(player.position - center.position) * 8f;
        transform.position = new Vector3(transform.position.x, transform.position.y + offset_y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.Equals(null))
        {
            radius = Vector3.Normalize(player.position - center.position) * 8f;
            float correction = Vector3.Angle(radius, transform.right);
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
            transform.position = new Vector3(player.position.x + radius.x, player.position.y + 2f, player.position.z + radius.z);
        }
        
    }
}
