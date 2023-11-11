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

        // Agrega un listener al evento onClick para cada botón
        boton.onClick.AddListener(PresionarBoton);
    }

    void PresionarBoton()
    {
        // Cambia la transparencia de la imagen cuando se presiona el botón
        Color color = imagen.color;
        color.a = 0.5f; // Cambia esto según lo opaco que quieras que sea el botón
        imagen.color = color;

        // Aquí puedes poner el código para la acción que deseas realizar al presionar el botón
        // Por ejemplo, si el botón "Ataque" hace daño al enemigo, aquí podrías llamar a una función de ataque.
    }
}
