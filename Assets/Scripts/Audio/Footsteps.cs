using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] stepSounds;
    [Range(0.0f, 1.0f)] public float volume = .25f;

    private float timeStepSoundPlayed;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (Time.time - timeStepSoundPlayed < 0.30f) return;
        int n = Random.Range(0, stepSounds.Length);
        AudioClip clip = stepSounds[n];
        audioSource.PlayOneShot(clip, volume);
        stepSounds[n] = stepSounds[0];
        stepSounds[0] = clip;
        timeStepSoundPlayed = Time.time;
    }
}
