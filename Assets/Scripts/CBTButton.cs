using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBTButton : MonoBehaviour
{
    private Button boton;
    private Image imagen;

    void Start()
    {
        boton = GetComponent<Button>();
        imagen = GetComponent<Image>();

        // Agrega un listener al evento onClick para cada bot�n
        boton.onClick.AddListener(PresionarBoton);
    }

    void PresionarBoton()
    {
        // Cambia la transparencia de la imagen cuando se presiona el bot�n
        Color color = imagen.color;
        color.a = 0.5f; // Cambia esto seg�n lo opaco que quieras que sea el bot�n
        imagen.color = color;

        // Aqu� puedes poner el c�digo para la acci�n que deseas realizar al presionar el bot�n
        // Por ejemplo, si el bot�n "Ataque" hace da�o al enemigo, aqu� podr�as llamar a una funci�n de ataque.
    }
}
