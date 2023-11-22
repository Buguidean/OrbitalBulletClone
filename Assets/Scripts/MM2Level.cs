using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Switchtolevels : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) | Input.GetKey(KeyCode.Keypad1) | Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
