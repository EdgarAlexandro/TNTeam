using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Barradellave : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI Contador;

    public void SetMaxKeys(int llave)
    {
        slider.maxValue = llave;
        slider.value = 0;
    }

    public void SetKeys(int llave)
    {
        slider.value = llave;
    }

    public void UpdateText(int textContent)
    {
        Contador.text = $"{textContent}";
    }
}
