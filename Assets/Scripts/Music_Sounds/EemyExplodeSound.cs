using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EemyExplodeSound : MonoBehaviour
{
    public bool explodeSound = false;

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
        if (explodeSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.2f);
            explodeSound = false;
        }
    }
}
