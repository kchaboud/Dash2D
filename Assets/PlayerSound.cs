using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource dashSrc;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip[] runSounds;
    public AudioClip dashSound;
    public AudioClip deathSound;
    public bool randomRunSounds = true;
    public float dashPitchFactor = 0.2f;

    public float runInterval = 1f;

    private AudioSource src;
    private PlayerController player;

    private float currentRunInterval = 0;
    private int runSoundIndex = 0;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        player = transform.parent.gameObject.GetComponent<PlayerController>();
        player.SetPlayerSound(this);
    }

    private void Update()
    {
        if (player.IsRunning() && IsRunIntervalReached())
        {
            PlayRunSound();
        }

        if (!player.IsRunning())
        {
            ResetRunInterval();
        }
    }

    private bool IsRunIntervalReached()
    {
        currentRunInterval -= Time.deltaTime;
        if (currentRunInterval <= 0f)
        {
            currentRunInterval = runInterval;
            return true;
        }
        return false;
    }

    private void ResetRunInterval()
    {
        currentRunInterval = 0f;
    }

    public void PlayJumpSound()
    {
        src.PlayOneShot(jumpSound);
    }

    public void PlayLandingSound()
    {
        src.PlayOneShot(landingSound);
    }

    public void PlayDashSound(int dashCount)
    {
        dashSrc.pitch = 1 + dashCount * dashPitchFactor;
        dashSrc.PlayOneShot(dashSound);
    }

    public void PlayRunSound()
    {
        AudioClip runSound;
        if (randomRunSounds)
        {
            runSound = runSounds[(int)Random.Range(0, runSounds.Length)];
        }
        else
        {
            runSound = runSounds[runSoundIndex];
            runSoundIndex = (runSoundIndex + 1) % runSounds.Length;
        }
        src.PlayOneShot(runSound);
    }

    public void PlayDeathSound()
    {
        src.PlayOneShot(deathSound);
    }

}
