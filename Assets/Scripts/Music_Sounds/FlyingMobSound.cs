using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMobSound : MonoBehaviour
{
    //public bool shootSound = false;
    public bool alertSound = false;

    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        /*if (shootSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.01f);
            shootSound = false;
        }*/
        if (alertSound)
        {
            controlAudio.PlayOneShot(audios[1], 0.1f);
            alertSound = false;
        }
    }
}
