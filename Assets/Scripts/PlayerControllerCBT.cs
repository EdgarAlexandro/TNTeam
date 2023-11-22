using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerControllerCBT : MonoBehaviour
{
    public GameObject botonAtaque;
    public GameObject botonJoker;
    public GameObject botonObjeto;
    public bool isAttacking = false;
    public Animator animator;
    public string reyActual;


    private GameObject[] botonesRPG;
    private int indexBotonSeleccionado = 0;

    // Start is called before the first frame update
    void Start()
    {
        botonAtaque = GameObject.Find("Ataque");
        botonJoker = GameObject.Find("Joker");
        botonObjeto = GameObject.Find("Objetos");
        animator = GetComponent<Animator>();


        // Añade los botones a la matriz en el orden que prefieras
        botonesRPG = new GameObject[] { botonAtaque, botonObjeto, botonJoker };
    }

    private void Update()
    {
        // Cambia el botón seleccionado
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            indexBotonSeleccionado++;
            if (indexBotonSeleccionado >= botonesRPG.Length) indexBotonSeleccionado = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            indexBotonSeleccionado--;
            if (indexBotonSeleccionado < 0) indexBotonSeleccionado = botonesRPG.Length - 1;
        }

        // Resalta el botón seleccionado y desactiva los otros
        for (int i = 0; i < botonesRPG.Length; i++)
        {
            if (i == indexBotonSeleccionado)
            {
                // Resalta el botón seleccionado
                botonesRPG[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                // Desactiva los otros botones
                botonesRPG[i].GetComponent<Button>().interactable = false;
            }
        }
        // Cambia el botón seleccionado y realiza una acción cuando se presiona la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Realiza una acción basada en el botón seleccionado
            switch (indexBotonSeleccionado)
            {
                case 0: // Ataque
                    Ataque();
                    break;
                case 1: // Joker
                        // Aquí puedes llamar a la función que realiza la animación de joker
                    break;
                case 2: // Objetos
                        // Aquí puedes llamar a la función que realiza la animación de objetos
                    break;
            }
        }
        void Ataque()
        {
            // Usa la variable reyActual para determinar qué animación reproducir
            switch (reyActual)
            {
                case "Reina de Corazones":
                    animator.Play("Reina de corazones Ataque");
                    break;
                case "Reina de Treboles":
                    animator.Play("Reina de treboles Ataque");
                    break;
                case "Rey de Diamantes":
                    animator.Play("Rey de diamantes Ataque");
                    break;
                case "Rey de Picas":
                    animator.Play("Rey de picas Ataque");
                    break;
                    
            }
        }
    }
}