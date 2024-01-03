using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChestSounds : MonoBehaviour
{
    public bool openSound = false;

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
        if (openSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.7f);
            openSound = false;
        }
    }
}
