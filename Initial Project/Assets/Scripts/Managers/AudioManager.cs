
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
    GameController gCont;
    //AudioSource audioSource;
    bool introComplete;
    bool mainMenu;
    public bool bossStageOne;

    public AudioSource[] MusicSources;

    void Awake()
    {
        

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.outputAudioMixerGroup = Music;
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

        MusicSources = GetComponents<AudioSource>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = Fx;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        gCont = FindObjectOfType<GameController>();

        int sceneID = SceneManager.GetActiveScene().buildIndex;

        if (sceneID > 2)
        {
            PlayMusic("IntroMainTheme");
        }
        else if (sceneID == 2)
        {
            mainMenu = true;
            PlayMusic("TutorialTheme");
        }
        else
        {
            mainMenu = true;
            PlayMusic("MenuTheme");
        }
    }

    void Update()
    {
        if (mainMenu == false)
        {
            if (!MusicSources[0].isPlaying)
            {
                if (!MusicSources[2].isPlaying && gCont.bossRoom == true)
                {
                    if (!MusicSources[4].isPlaying && bossStageOne == false)
                    {
                        bossStageOne = true;
                        PlayMusic("BossTheme01");
                    }
                    if (!MusicSources[4].isPlaying && !MusicSources[5].isPlaying && bossStageOne == true)
                    {
                        PlayMusic("BossTheme02");
                    }
                }
                else
                {
                    if (!MusicSources[2].isPlaying)
                    {
                        PlayMusic("MainTheme");
                    }
                }

            }
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
