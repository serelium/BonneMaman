using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] string[] stepSounds;

    public void PlayRandomFootstepSound()
    {
        int n = Random.Range(0, stepSounds.Length);

        AudioManager.Instance.Play(gameObject, stepSounds[n]);
    }
}
