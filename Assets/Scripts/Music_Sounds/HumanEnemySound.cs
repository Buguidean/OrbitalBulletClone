using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemySound : MonoBehaviour
{
    public bool shootSound = false;
    public bool alertSound = false;

    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (shootSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.1f);
            shootSound = false;
        }
        else if (alertSound)
        {
            controlAudio.PlayOneShot(audios[1], 0.1f);
            alertSound = false;
        }
    }
}
