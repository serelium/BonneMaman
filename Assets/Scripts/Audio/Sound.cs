using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 1;

    [Range(0.1f, 3f)]
    public float Pitch = 0;

    [HideInInspector]
    public AudioSource Source;
    public bool Loop;
}
