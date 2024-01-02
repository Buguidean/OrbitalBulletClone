using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_spider : MonoBehaviour
{
    public Transform spider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spider != null)
        {
            gameObject.transform.position = new Vector3(spider.position.x,spider.position.y + 3f,spider.position.z);
        }
    }
}
