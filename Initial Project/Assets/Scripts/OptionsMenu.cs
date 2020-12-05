using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    Resolution[] resolutions;

    public AudioMixer audioMixer;
    public AudioConfiguration Config;
    public TMP_Dropdown resolutionDropdown;

    public Slider fxSlider, musicSlider, masterSlider;

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pauseFirst;

    public float fxVol, musicVol, masterVol;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        float currentmasterVol = PlayerPrefs.GetFloat("masterVol", 0);
        float currentmusicVol = PlayerPrefs.GetFloat("musicVol", 0);
        float currentfxVol = PlayerPrefs.GetFloat("fxVol", 0);

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        fxSlider.value = currentfxVol;
        musicSlider.value = currentmusicVol;
        masterSlider.value = currentmasterVol;

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        keys.Add("Escape", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape")));
    }

    public void Update()
    {
        if (Input.GetKeyDown(keys["Escape"]))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

    public void SetMasterVolume(float MasterVolume)
    {
        audioMixer.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("masterVol", MasterVolume);
    }

    public void SetMusicVolume(float MusicVolume)
    {
        audioMixer.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("musicVol", MusicVolume);
    }

    public void SetEffectsVolume(float EffectsVolume)
    {
        audioMixer.SetFloat("EffectsVolume", EffectsVolume);
        PlayerPrefs.SetFloat("fxVol", EffectsVolume);
    }

    public void SetQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitMenu()
    {
        PlayerPrefs.Save();
    }

}
