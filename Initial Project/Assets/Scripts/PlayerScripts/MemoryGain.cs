using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGain : MonoBehaviour
{
    PlayerController pCont;
    GameController gCont;
    OptionsMenu Options;

    public Sprite image;

    void Start()
    {
        pCont = FindObjectOfType<PlayerController>();
        gCont = FindObjectOfType<GameController>();
        Options = FindObjectOfType<OptionsMenu>();
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sprite = image;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GainMemory();
        FindObjectOfType<AudioManager>().Play("Pickup");
        Destroy(gameObject);
    }

    void GainMemory()
    {
        gCont.StartCoroutine(gCont.MemoryAdded());
        if (PlayerPrefs.GetInt("Memory1", 0) == 0)
        {
            PlayerPrefs.SetInt("Memory1", 1);
        }

        else if (PlayerPrefs.GetInt("Memory1", 0) == 1)
        {
            PlayerPrefs.SetInt("Memory2", 1);
        }

        else if (PlayerPrefs.GetInt("Memory2", 0) == 1)
        {
            PlayerPrefs.SetInt("Memory3", 1);
            gCont.allMemories = true;
        }
        PlayerPrefs.Save();
        Options.MemoryCheck();
    }
}
