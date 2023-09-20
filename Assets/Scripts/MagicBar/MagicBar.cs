using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI magicText;

    public void SetMaxMagic(int magic)
    {
        slider.maxValue = magic;
        slider.value = 0;
    }

    public void SetMagic(int magic)
    {
        slider.value = magic;
    }

    public void UpdateText(int textContent)
    {
        magicText.text = $"{textContent}";
    }
}
