using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationSound : MonoBehaviour
{
    public bool congratsSound = false;

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
        if (congratsSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.2f);
            congratsSound = false;
        }
    }
}
