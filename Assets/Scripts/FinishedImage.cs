using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedImage : MonoBehaviour
{
    private float timer;

    void Start()
    {
        timer = 3f;
        gameObject.GetComponent<CongratulationSound>().congratsSound = true;
    }

    void FixedUpdate()
    {
        if (timer <= 0f)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        timer -= Time.deltaTime;
    }
}
