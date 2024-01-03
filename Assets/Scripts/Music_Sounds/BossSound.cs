using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    public bool shotSound = false;
    public bool jumpSound = false;

    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shotSound)
        {
            if (!controlAudio.isPlaying) {
                controlAudio.PlayOneShot(audios[0], 0.1f);
                shotSound = false;
            }
        }
        if (jumpSound)
        {
            controlAudio.PlayOneShot(audios[1], 1f);
            jumpSound = false;
        }
    }
}
