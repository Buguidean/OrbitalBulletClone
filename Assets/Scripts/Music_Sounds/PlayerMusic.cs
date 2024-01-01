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
            stopMusic = false;
            controlAudio.Stop();
        }
        if (!controlAudio.isPlaying)
        {
            if (isStageMusic)
            {
                controlAudio.PlayOneShot(audios[0], 0.1f);
                isStageMusic = false;
            }
            else if (isBossMusic)
            {
                controlAudio.PlayOneShot(audios[1], 0.1f);
                isBossMusic = false;
            }
        }
    }
}
