using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMobSound : MonoBehaviour
{
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
        if (alertSound)
        {
            controlAudio.PlayOneShot(audios[1], 0.1f);
            alertSound = false;
        }
    }
}
