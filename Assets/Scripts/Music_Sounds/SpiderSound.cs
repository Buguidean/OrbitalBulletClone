using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSound : MonoBehaviour
{

    public bool attackSound = false;

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
        if (attackSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.2f);
            attackSound = false;
        }
    }
}
