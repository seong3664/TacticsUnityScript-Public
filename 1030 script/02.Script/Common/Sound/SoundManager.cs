using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource audioSource;
    public AudioClip gunshoot;
    public AudioClip ReLoad;
    public AudioClip HIT;
    public AudioClip Miss;
    public AudioClip footstep;
    public AudioClip background;
    public AudioClip ClickSound;
    public AudioClip BtnSound;
    Dictionary<string, AudioClip> soundclips;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
       audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        soundclips = new Dictionary<string, AudioClip>()
    {
        { "gunshoot", gunshoot },
        { "footstep", footstep },
        { "Miss", Miss},
        { "Hit", HIT },
        { "ReLoad", ReLoad },
        { "background", background },
        { "ClickSound", ClickSound },
        { "BtnSound", BtnSound }
    };
    }
    public void PlayerSound(string name)
    {
        audioSource.PlayOneShot(soundclips[name]);
    }
}
