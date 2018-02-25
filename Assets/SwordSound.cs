using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSound : MonoBehaviour {
    public AudioClip[] swordSwingSounds;
    public bool randomSwingSound = true;
    private AudioSource src;
    private int swingSoundIndex = 0;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlaySwingSound()
    {
        AudioClip swingSound;
        if (randomSwingSound)
        {
            swingSound = swordSwingSounds[(int)Random.Range(0, swordSwingSounds.Length)];
        }
        else
        {
            swingSound = swordSwingSounds[swingSoundIndex];
            swingSoundIndex = (swingSoundIndex + 1) % swordSwingSounds.Length;
        }
        src.PlayOneShot(swingSound);
    }
}
