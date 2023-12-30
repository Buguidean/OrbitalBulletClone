using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip audio;

    private AudioSource controlAudio;
    private float timerGameOver = 3.3f;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
        controlAudio.loop = false;
        controlAudio.PlayOneShot(audio, 0.2f);
    }

    void FixedUpdate()
    {
        if(timerGameOver == 0f)
            SceneManager.LoadScene(0, LoadSceneMode.Single);

        timerGameOver -= Time.deltaTime;
        if (timerGameOver < 0f)
            timerGameOver = 0f;
    }

}
