using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusic : MonoBehaviour
{
    public bool isBossMusic = false;
    public bool isStageMusic = true;
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip[] audios;

    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
        //controlAudio.loop = true;
    }

    private void FixedUpdate()
    {
        if (!controlAudio.isPlaying)
        {
            if (isStageMusic)
            {
                controlAudio.PlayOneShot(audios[0], 0.1f);
            }
            else if (isBossMusic)
            {
                controlAudio.PlayOneShot(audios[1], 0.1f);
            }
        }
    }
}
