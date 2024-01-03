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
    public bool takeWeaponSound = false;
    public bool stageClear = false;

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
                controlAudio.PlayOneShot(audios[0], 8f);
                jumpSound = false;
                //Debug.Log("begin of the jumpSound");
            }
            if (stompSound)
            {
                controlAudio.PlayOneShot(audios[1], 8f);
                stompSound = false;
            }
            if (pistolSound)
            {
                controlAudio.PlayOneShot(audios[2], 2f);
                pistolSound = false;
            }
            if (rifleSound)
            {
                controlAudio.PlayOneShot(audios[3], 1.6f);
                rifleSound = false;
            }
            if (collectAmmoSound)
            {
                controlAudio.PlayOneShot(audios[4], 5f);
                collectAmmoSound = false;
            }
            if (noAmmoSound)
            {
                controlAudio.PlayOneShot(audios[5], 1.5f);
                noAmmoSound = false;
            }
            if (teleport)
            {
                controlAudio.PlayOneShot(audios[6], 6f);
                teleport = false;
            }
            if (transitionSound)
            {
                controlAudio.PlayOneShot(audios[7], 6f);
                transitionSound = false;
            }
            if (changeWeaponSound)
            {
                controlAudio.PlayOneShot(audios[8], 1f);
                changeWeaponSound = false;
            }
            if (gruntSound)
            {
                controlAudio.PlayOneShot(audios[9], 2.2f);
                gruntSound = false;
            }
            if(takeWeaponSound)
            {
                controlAudio.PlayOneShot(audios[11], 2.6f);
                takeWeaponSound = false;
            }
            if (stageClear)
            {
                controlAudio.PlayOneShot(audios[12], 20f);
                stageClear = false;
            }
        }
        else if (dyingSound)
        {
            controlAudio.PlayOneShot(audios[10], 3f);
            dyingSound = false;
        }

        /*if (gameOverSound)
        {
            controlAudio.PlayOneShot(audios[11], 1f);
            gameOverSound = false;
        }*/
    }

}
