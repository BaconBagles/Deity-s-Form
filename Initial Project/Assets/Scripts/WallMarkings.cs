using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMarkings : MonoBehaviour
{

    public GameObject currentGameObject;
    public float alpha = 0.5f;
    void Start()
    {
        currentGameObject = gameObject;
    }

    void Update()
    {
        ChangeAlpha(currentGameObject.GetComponent<Renderer>().material, alpha);
    }

    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);

    }
}
