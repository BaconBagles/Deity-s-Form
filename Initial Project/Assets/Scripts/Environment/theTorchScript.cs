using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class theTorchScript : MonoBehaviour
{
    public Light2D mylight;
    public GameObject torchparticles;

    void Start()
    {
        mylight = GetComponent<Light2D>();
        mylight.enabled = true;
        torchparticles = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        torchparticles.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BullSpecial"))
        {
            mylight.enabled = false;
            torchparticles.SetActive(false);
        }
    }

    public void LightsOut()
    {
        mylight.enabled = false;
        torchparticles.SetActive(false);
    }
}
