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
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
