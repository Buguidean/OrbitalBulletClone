using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedImage : MonoBehaviour
{
    float timer = 3f;

    void FixedUpdate()
    {
        if (timer <= 0f)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        timer -= Time.deltaTime;
    }
}
