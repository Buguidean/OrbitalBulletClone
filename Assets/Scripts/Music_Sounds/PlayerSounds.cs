using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds: MonoBehaviour
{
    public bool jumpSound = false;
    public bool stompSound = false;
    public bool pistolSound = false;
    public bool rifleSound = false;
    public bool collectAmmoSound = false;
    public bool noAmmoSound = false;
    public bool teleport = false;
    public bool transitionSound = false;
    public bool changeWeaponSound = false;
    public bool gruntSound = false;
    public bool dyingSound = false;
    public bool gameOverSound = false;

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
        if (!dyingSound)
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
            if (pistolSound)
            {
                controlAudio.PlayOneShot(audios[2], 0.2f);
                pistolSound = false;
            }
            if (rifleSound)
            {
                controlAudio.PlayOneShot(audios[3], 0.2f);
                rifleSound = false;
            }
            if (collectAmmoSound)
            {
                controlAudio.PlayOneShot(audios[4], 1);
                collectAmmoSound = false;
            }
            if (noAmmoSound)
            {
                controlAudio.PlayOneShot(audios[5], 0.5f);
                noAmmoSound = false;
            }
            if (teleport)
            {
                controlAudio.PlayOneShot(audios[6], 1);
                teleport = false;
            }
            if (transitionSound)
            {
                controlAudio.PlayOneShot(audios[7], 1);
                transitionSound = false;
            }
            if (changeWeaponSound)
            {
                controlAudio.PlayOneShot(audios[8], 0.5f);
                changeWeaponSound = false;
            }
            if (gruntSound)
            {
                controlAudio.PlayOneShot(audios[9], 0.5f);
                gruntSound = false;
            }
        }
        else if (dyingSound)
        {
            controlAudio.PlayOneShot(audios[10], 0.5f);
            dyingSound = false;
        }

        /*if (gameOverSound)
        {
            controlAudio.PlayOneShot(audios[11], 1f);
            gameOverSound = false;
        }*/
    }

}
