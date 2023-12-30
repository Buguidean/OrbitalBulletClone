using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemySound : MonoBehaviour
{
    public bool shootSound = false;

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
            controlAudio.PlayOneShot(audios[0], 0.3f);
            shootSound = false;
        }
    }
}
