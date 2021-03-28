using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryCooldown : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;

    public void SetMaxTime(int time)
    {
        slider.maxValue = time;
        slider.value = time;
    }

    public void SetTime(float time)
    {
        slider.value = time;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
