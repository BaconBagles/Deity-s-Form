
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager instance;
    public AudioMixerGroup Fx, Music;
    public Sound[] sounds, music;
    public OptionsMenu Options;
    AudioSource audioSource;
    bool introComplete;

    private void Start()
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex;

        if (sceneID > 0)
        {
            PlayMusic("IntroMainTheme");
        }
        else
        {
            PlayMusic("MenuTheme");
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
      */

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.outputAudioMixerGroup = Music;
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = Fx;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        
    }

    void Update()
    {
        if (!audioSource.isPlaying && introComplete == false)
        {
            introComplete = true;
            PlayMusic("MainTheme");
        }
    }

    public void Play(string name)
    {
        if (Options.GameIsPaused == false)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Play();
        }
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
          if (m == null)
          {
              Debug.LogWarning("Sound: " + name + " not found!");
              return;
          }
          m.source.Play();

    }
}
