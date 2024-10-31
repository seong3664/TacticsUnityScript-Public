using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip ChangeTurn;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
        soundclips = new Dictionary<string, AudioClip>()
    {
        { "gunshoot", gunshoot },
        { "footstep", footstep },
        { "Miss", Miss},
        { "Hit", HIT },
        { "ReLoad", ReLoad },
        { "background", background },
        { "ClickSound", ClickSound },
        { "BtnSound", BtnSound },
        { "ChangeTurn", ChangeTurn }
    };
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle_Scenes")
        {
            audioSource.clip = background;
        }
        audioSource.Play();
        
    }
    public void PlayerSound(string name)
    {
        audioSource.PlayOneShot(soundclips[name]);
    }
}
