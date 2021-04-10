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
    public TMP_ColorGradient[] gradients;
    public TMP_FontAsset[] fonts;

    public AudioMixer audioMixer;
    public AudioConfiguration Config;
    public TMP_Dropdown resolutionDropdown, textColourDropdown, fontDropdown;
    public TextMeshProUGUI[] allText;

    public Slider fxSlider, musicSlider, masterSlider, playerHealthSlider, turnTimerSlider, enemyHealthSlider;

    public int playerHealth, enemyHealth, turnTimer, memory1, memory2, memory3;

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI, memoryText1, memoryText2, memoryText3;
    public GameObject pauseFirst;

    public float fxVol, musicVol, masterVol;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        fxSlider.value = PlayerPrefs.GetFloat("masterVol", 0);
        musicSlider.value = PlayerPrefs.GetFloat("musicVol", -10);
        masterSlider.value = PlayerPrefs.GetFloat("fxVol", 0);
        playerHealthSlider.value = PlayerPrefs.GetInt("playerHeath", 60);
        turnTimerSlider.value = PlayerPrefs.GetInt("turnTimer", 5); 
        enemyHealthSlider.value = PlayerPrefs.GetInt("enemyHealth", 5);
        
        if (memory1 != 1)
        {
            PlayerPrefs.SetInt("Memory1", 0);
        }
        if (memory2 != 1)
        {
            PlayerPrefs.SetInt("Memory2", 0);
        }
        if (memory3 != 1)
        {
            PlayerPrefs.SetInt("Memory3", 0);
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        allText = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)) as TextMeshProUGUI[];
        textColourDropdown.value = PlayerPrefs.GetInt("gradient", 0);
        fontDropdown.value = PlayerPrefs.GetInt("font", 0);

        keys.Add("Escape", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape")));
        MemoryCheck();
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

    public void MemoryCheck()
    {
        memory1 = PlayerPrefs.GetInt("Memory1", 0);
        memory2 = PlayerPrefs.GetInt("Memory2", 0);
        memory3 = PlayerPrefs.GetInt("Memory3", 0);
        PlayerPrefs.Save();

        if (memory1 == 1)
        {
            memoryText1.SetActive(true);
        }

        if (memory2 == 1)
        {
            memoryText2.SetActive(true);
        }

        if (memory3 == 1)
        {
            memoryText3.SetActive(true);
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

    public void SetGameplay()
    {
        turnTimer = (int)turnTimerSlider.value;
        enemyHealth = (int)enemyHealthSlider.value;
        playerHealth = (int)playerHealthSlider.value;

        PlayerPrefs.SetInt("turnTimer", turnTimer);
        PlayerPrefs.SetInt("playerHealth", playerHealth);
        PlayerPrefs.SetInt("enemyHealth", enemyHealth);

        PlayerPrefs.Save();
    }

    public void SetTextFont(int font)
    {
        foreach (TextMeshProUGUI text in allText)
        {
            text.font = fonts[font];
        }
        PlayerPrefs.SetInt("font", font);
    }

    public void SetTextColour(int gradientIndex)
    {
        foreach (TextMeshProUGUI text in allText)
        {
            text.colorGradientPreset = gradients[gradientIndex];
        }

        PlayerPrefs.SetInt("gradient", gradientIndex);
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
