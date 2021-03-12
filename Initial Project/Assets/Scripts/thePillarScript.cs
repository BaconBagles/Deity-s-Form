using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thePillarScript : MonoBehaviour
{
    public int pillarState;
    public SpriteRenderer spriteRenderer;
    public Sprite[] state;

    void Start()
    {
        pillarState = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BullSpecial"))
        {
            Debug.Log("Damage");
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
                Destroy(this.gameObject);
                break;
            case 1:
                spriteRenderer.sprite = state[2];
                break;
            case 2:
                spriteRenderer.sprite = state[1];
                break;
            case 3:
                spriteRenderer.sprite = state[0];
                break;
        }
    }

   
}
