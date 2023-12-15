using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform center;

    private Vector2 radius;
    private Vector2 center2D;
    private Vector2 player2D;
    private Vector2 result;
   
    private float offset_y = 2.55f;
    private float offset_dist = 16.7f;

    void Start()
    {
        center2D = new Vector2(center.position.x, center.position.z);
        player2D = new Vector2(player.position.x, player.position.z);
        result = player2D - center2D;
        result.Normalize();
        radius = result * offset_dist;
        transform.position = new Vector3(transform.position.x, transform.position.y + offset_y, transform.position.z);
        //transform.Rotate(0.0f, 0.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.Equals(null))
        {
            player2D = new Vector2(player.position.x, player.position.z);
            result = player2D - center2D;
            result.Normalize();
            radius = result * offset_dist;
            float correction = Vector3.Angle(new Vector3(radius.x, 0f ,radius.y), transform.right);
            transform.Rotate(0.0f, correction - 90.0f, 0.0f);
            transform.position = new Vector3(player.position.x + radius.x, player.position.y + offset_y, player.position.z + radius.y);
        }
        
    }
}
