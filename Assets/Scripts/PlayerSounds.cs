using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds: MonoBehaviour
{
    public bool jumpSound = false;
    public bool stompSound = false;
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (jumpSound)
        {
            controlAudio.PlayOneShot(audios[0], 1);
            jumpSound = false;
            //Debug.Log("begin of the jumpSound");
        }
        if (stompSound)
        {
            controlAudio.PlayOneShot(audios[1], 1);
            stompSound = false;
        }

    }

}
