using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonDisableScript : MonoBehaviour
{
    [SerializeField] private GameObject confirmScreen;
    private Button thisButton;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = this.gameObject.GetComponent<Button>();
        thisButton.enabled = true;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (confirmScreen == null)
        {
            thisButton.enabled = true;
        }
        else if(confirmScreen != null)
        {
            thisButton.enabled = false;
        }
        confirmScreen = GameObject.FindGameObjectWithTag("AreYouSure");
    }
}
