using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusic : MonoBehaviour
{
    public bool isBossMusic = false;
    public bool isStageMusic = true;
    public bool stopMusic = false;

    // Start is called before the first frame update
    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
        controlAudio.loop = true;
    }

    private void FixedUpdate()
    {
        if (stopMusic)
        {
            controlAudio.Stop();
            stopMusic = false;
        }
        if (isStageMusic)
        {
            controlAudio.Stop();
            controlAudio.clip = audios[0];
            controlAudio.loop = true;
            controlAudio.volume = 0.05f;
            controlAudio.Play();
            isStageMusic = false;
        }
        else if (isBossMusic)
        {
            controlAudio.Stop();
            controlAudio.clip = audios[1];
            controlAudio.loop = true;
            controlAudio.volume = 0.05f;
            controlAudio.Play();
            isBossMusic = false;
        }
    }
}
