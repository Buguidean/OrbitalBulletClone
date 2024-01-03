using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits2MM : MonoBehaviour
{
    private float timer = 4f;
    
    void FixedUpdate()
    {
        if(timer <= 0f)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        timer -= Time.deltaTime;
    }
}
