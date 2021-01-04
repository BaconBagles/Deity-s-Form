using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class KeybindsManager : MonoBehaviour
{
    #region Fields
    //Dictionary for storing keybinds
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI upText, leftText, downText, rightText, escapetext, /*attackupText, attackleftText, attackdownText, attackrightText, form1Text, form2Text, form3Text,*/ switchAText, switchBText, basicAttackText, secondaryAttackText; //Accessing text on menu buttons

    private GameObject currentKey;

    private Color32 normal = Color.white;
    private Color32 selected = new Color32(170, 231, 243, 255); //light blue
    #endregion

    #region Methods
    #region UnityMethods

    // Start is called before the first frame update
    void Start()
    {

        //Add the saved PlayerPrefs for the keys to the Dictionary OR use the default values if there's nothing saved
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Escape", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape")));
       /* keys.Add("AttackUp", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackUp", "UpArrow")));
        keys.Add("AttackDown", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackDown", "DownArrow")));
        keys.Add("AttackLeft", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackLeft", "LeftArrow")));
        keys.Add("AttackRight", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackRight", "RightArrow")));
        keys.Add("form1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form1", "1")));
        keys.Add("form2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form2", "2")));
        keys.Add("form3", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form3", "3")));*/
        keys.Add("switchA", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchA", "Q")));
        keys.Add("switchB", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchB", "E")));
        keys.Add("basicAttack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("basicAttack", "LeftMouse")));
        keys.Add("secondaryAttack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("secondaryAttack", "RightMouse")));

        //Change the button text to match the keybinds
        upText.text = keys["Up"].ToString();
        downText.text = keys["Down"].ToString();
        leftText.text = keys["Left"].ToString();
        rightText.text = keys["Right"].ToString();
        escapetext.text = keys["Escape"].ToString();
       /* attackupText.text = keys["AttackUp"].ToString();
        attackdownText.text = keys["AttackDown"].ToString();
        attackleftText.text = keys["AttackLeft"].ToString();
        attackrightText.text = keys["AttackRight"].ToString();
        form1Text.text = keys["form1"].ToString();
        form2Text.text = keys["form2"].ToString();
        form3Text.text = keys["form3"].ToString();*/
        switchAText.text = keys["switchA"].ToString();
        switchBText.text = keys["switchB"].ToString();
        basicAttackText.text = keys["basicAttack"].ToString();
        secondaryAttackText.text = keys["secondaryAttack"].ToString();

    }

    private void OnGUI()
    {
        //Check if there is any currentKey selected
        if (currentKey != null)
        {
            Event e = Event.current;
            KeyCode input = e.keyCode;
            //Checks that the currentKey is actually a key
            //and the input isn't already being used for another key
            if (e.isKey
                && input != keys["Up"]
                && input != keys["Down"]
                && input != keys["Left"]
                && input != keys["Right"]
                && input != keys["Escape"]
                /*&& input != keys["AttackUp"]
                && input != keys["AttackDown"]
                && input != keys["AttackLeft"]
                && input != keys["AttackRight"]
                && input != keys["form1"]
                && input != keys["form2"]
                && input != keys["form3"]*/
                && input != keys["switchA"]
                && input != keys["switchB"]
                && input != keys["basicAttack"]
                && input != keys["secondaryAttackText"]
                )
            {
                //Change the currentKey's name to the key that was just pressed
                keys[currentKey.name] = e.keyCode;
                //Change the text to match the new name
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                //Changes the currentKey color back to normal
                currentKey.GetComponent<Image>().color = normal;

                currentKey = null;
            }
        }
    }
    #endregion
    public void ChangeKey(GameObject clicked)
    {
        //Checks if you've already selected a key
        if (currentKey != null)
        {
            //Change other keys back to their normal color
            currentKey.GetComponent<Image>().color = normal;
        }

        //Sets the current key to the gameObject that was clicked
        currentKey = clicked;
        //Changes the colour to highlight the currentKey
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        //For each key in the Dictionary keys
        foreach (var key in keys)
        {
            //Set the value (which is a string) of the current key
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void SetDefaultKeys()
    {
        //Quickly sets the keys to a standard configuration
        keys["Up"] = KeyCode.W;
        keys["Down"] = KeyCode.S;
        keys["Left"] = KeyCode.A;
        keys["Right"] = KeyCode.D;
        keys["Escape"] = KeyCode.Escape;
        /*keys["AttackUp"] = KeyCode.UpArrow;
        keys["AttackDown"] = KeyCode.DownArrow;
        keys["AttackLeft"] = KeyCode.LeftArrow;
        keys["AttackRight"] = KeyCode.RightArrow;
        keys["form1"] = KeyCode.Alpha1;
        keys["form2"] = KeyCode.Alpha2;
        keys["form3"] = KeyCode.Alpha3;*/
        keys["switchA"] = KeyCode.Q;
        keys["switchB"] = KeyCode.E;
        keys["basicAttack"] = KeyCode.Mouse0;
        keys["secondaryAttack"] = KeyCode.Mouse1;

        //Updated the text to match the new keybinds
        upText.text = keys["Up"].ToString();
        downText.text = keys["Down"].ToString();
        leftText.text = keys["Left"].ToString();
        rightText.text = keys["Right"].ToString();
        escapetext.text = keys["Escape"].ToString();
        /*attackupText.text = keys["AttackUp"].ToString();
        attackdownText.text = keys["AttackDown"].ToString();
        attackleftText.text = keys["AttackLeft"].ToString();
        attackrightText.text = keys["AttackRight"].ToString();
        form1Text.text = keys["form1"].ToString();
        form2Text.text = keys["form2"].ToString();
        form3Text.text = keys["from3"].ToString();*/
        switchAText.text = keys["switchA"].ToString();
        switchBText.text = keys["switchB"].ToString();
        basicAttackText.text = keys["basicAttack"].ToString();
        secondaryAttackText.text = keys["secondaryAttack"].ToString();
    }
    
    public void ExitKeybindMenu()
    {
        //if current values do NOT equal player prefs, save the new values
        if (keys["Up"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W"))
            || keys["Down"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S"))
            || keys["Left"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"))
            || keys["Right"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"))
            || keys["Escape"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Escape", "Escape"))
            /*|| keys["AttackUp"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackUp", "UpArrow"))
            || keys["AttackDown"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackDown", "DownArrow"))
            || keys["AttackLeft"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackLeft", "LeftArrow"))
            || keys["AttackRight"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackRight", "RightArrow"))
            || keys["form1"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form1", "1"))
            || keys["form2"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form2", "2"))
            || keys["form3"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("form3", "3"))*/
            || keys["switchA"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchA", "Q"))
            || keys["switchB"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchB", "E"))
            || keys["basicAttack"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("basicAttack", "Left Mouse"))
            || keys["secondaryAttack"] != (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("secondaryAttack", "Right Mouse"))
            )
        {
            SaveKeys();
        }

    }
   
    #endregion
}
