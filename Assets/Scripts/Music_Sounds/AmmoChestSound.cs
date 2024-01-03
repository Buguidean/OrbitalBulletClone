using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoChestSound : MonoBehaviour
{
    public bool destroySound = false;

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
        if (destroySound)
        {
            controlAudio.PlayOneShot(audios[0], 1);
            destroySound = false;
            //Debug.Log("begin of the jumpSound");
        }
    }
}
