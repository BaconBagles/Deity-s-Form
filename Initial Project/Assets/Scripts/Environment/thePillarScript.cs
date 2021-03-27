using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thePillarScript : MonoBehaviour
{
    public int pillarState;
    public SpriteRenderer spriteRenderer;
    public Sprite[] state;
    public CircleCollider2D mycollider;
    public GameObject mylight;

    void Start()
    {
        pillarState = state.Length-1;
        mycollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mylight = this.gameObject.transform.GetChild(0).gameObject;
        mylight.SetActive(true);
        mycollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BullSpecial"))
        {
            pillarState -= 2;

            PillarDamage();
        }
    }

    public void PillarDamage()
    {
        // CALL PARTICLES HERE 
        if (pillarState < 0)
        {
            pillarState = 0;
        }

        switch (pillarState)
        {
            case 0:
                CameraShake cam = FindObjectOfType<CameraShake>();
                cam.StartCoroutine(cam.Shake(0.3f, 0.8f));
                spriteRenderer.sprite = state[pillarState];
                mylight.SetActive(false);
                mycollider.enabled = false;
                break;
            case 1:
                spriteRenderer.sprite = state[pillarState];
                break;
            case 2:
                spriteRenderer.sprite = state[pillarState];
                break;
            case 3:
                spriteRenderer.sprite = state[pillarState];
                break;
        }
    }

   
}
