using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public OptionsMenu options;
    public GameController game;
    public AudioManager Audio;

    public AudioSource[] allaudio;

    public TextMeshProUGUI deathText;
    public GameObject deathButton, deathBackground;

    public bool dead;

    void Start()
    {
        dead = false;
    }

    void Update()
    { 
        if (dead == true)
        {
            allaudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource all in allaudio)
            {
                all.Stop();
            }

            GameOver();
            dead = false;
        }
    }

    void GameOver()
    {
        Audio.PlayMusic("GameOver");
        EventSystem.current.SetSelectedGameObject(deathButton);
        deathBackground.SetActive(true);
        Time.timeScale = 0f;
        options.GameIsPaused = true;
        deathText.text = "You cleared " + game.currentRoom + " rooms";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
