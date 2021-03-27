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
        torchparticles = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        LightsOn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BullSpecial"))
        {
            LightsOut();
        }
    }

    public void LightsOut()
    {
        mylight.enabled = false;
        if(torchparticles != null)
        {
        torchparticles.SetActive(false);
        }
    }

    public void LightsOn()
    {
        mylight.enabled = true;
        if (torchparticles != null)
        {
            torchparticles.SetActive(true);
        }
    }
}
