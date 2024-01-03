using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeExplode : MonoBehaviour
{
    public bool explosionSound = false;

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
        if (explosionSound)
        {
            controlAudio.PlayOneShot(audios[0], 0.5f);
            Debug.Log("Hace el sonido de explosión");
            explosionSound = false;
        }
    }
}
