using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip audio;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
        controlAudio.loop = false;
        controlAudio.PlayOneShot(audio, 0.2f);
    }
}
