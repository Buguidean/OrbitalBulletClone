using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pos : MonoBehaviour
{
    public new Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.position;
    }
}
