using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<Sound> soundLibrary;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public void Play(GameObject owner, string name)
    {
        Sound sound = soundLibrary.Find(s => s.Name == name);

        if(sound == null)
        {
            Debug.LogWarning($"Sound: {name} does not exist.");
        }

        sound.Source = owner.AddComponent<AudioSource>();
        sound.Source.clip = sound.Clip;
        sound.Source.volume = sound.Volume;
        sound.Source.pitch = sound.Pitch;
        sound.Source.Play();

        StartCoroutine(DestroyAfterPlay(sound.Source));
    }

    private IEnumerator DestroyAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        Destroy(source);
    }
}
