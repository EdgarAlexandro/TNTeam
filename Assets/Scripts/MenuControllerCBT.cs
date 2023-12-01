using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuControllerCBT : MonoBehaviour
{
    private GameObject botonAtaque;
    private GameObject botonJoker;
    private GameObject botonObjeto;
    private GameObject botonPocionCuracion;
    private GameObject botonPocionMagia;
    private GameObject botonPocionRevivir;
    private GameObject botonBomba;
    private GameObject botonExit;
    public bool canControl = false;
    public bool objectsMenuActive = false;

    private Animator animator;
    private GameObject[] botonesRPG;
    private GameObject[] botonesObjetos;
    private int indexBotonSeleccionado = 0;
    private int indexBotonSeleccionadoObjetos = 0;
    private bool buttonsSet = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("GetButtons");
        animator = GetComponent<Animator>();
        GetObjectButtons();

        // Añade los botones a la matriz en el orden que prefieras
    }


    public void GetButtons(GameObject attack, GameObject joker, GameObject objects)
    {
        botonAtaque = attack;
        botonJoker = joker;
        botonObjeto = objects;

        botonesRPG = new GameObject[] { botonAtaque, botonObjeto, botonJoker };
        buttonsSet = true;
    }

    public void GetObjectButtons()
    {
        GameObject objectsMenu = GameObject.Find("ObjectsMenu").transform.GetChild(0).gameObject;
        botonPocionCuracion = objectsMenu.transform.GetChild(0).gameObject; 
        botonPocionMagia = objectsMenu.transform.GetChild(1).gameObject;
        botonPocionRevivir = objectsMenu.transform.GetChild(2).gameObject;
        botonBomba = objectsMenu.transform.GetChild(3).gameObject;
        botonExit = objectsMenu.transform.GetChild(4).gameObject;

        botonesObjetos = new GameObject[] { botonPocionCuracion, botonPocionMagia, botonPocionRevivir, botonBomba, botonExit };
    }

    void ButtonNavigationUp()
    {
        if (botonesObjetos[indexBotonSeleccionadoObjetos].name != "Exit" && botonesObjetos[indexBotonSeleccionadoObjetos].GetComponent<ObjectsMenuButton>().uses == 0)
        {
            indexBotonSeleccionadoObjetos--;
            if (indexBotonSeleccionadoObjetos < 0) indexBotonSeleccionadoObjetos = botonesObjetos.Length - 1;
            ButtonNavigationUp();
        }
    }

    void ButtonNavigationDown()
    {
        if (botonesObjetos[indexBotonSeleccionadoObjetos].name != "Exit" && botonesObjetos[indexBotonSeleccionadoObjetos].GetComponent<ObjectsMenuButton>().uses == 0)
        {
            indexBotonSeleccionadoObjetos++;
            if (indexBotonSeleccionadoObjetos >= botonesObjetos.Length) indexBotonSeleccionadoObjetos = 0;
            ButtonNavigationDown();
        }
    }

    private void Update()
    {
        if (buttonsSet && canControl)
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
                botonesRPG[indexBotonSeleccionado].GetComponent<Button>().onClick.Invoke();
            }
        }else if (objectsMenuActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                indexBotonSeleccionadoObjetos--;
                if (indexBotonSeleccionadoObjetos < 0) indexBotonSeleccionadoObjetos = botonesObjetos.Length - 1;
                ButtonNavigationUp();
                //if (botonesObjetos[indexBotonSeleccionadoObjetos].name != "Exit" && botonesObjetos[indexBotonSeleccionadoObjetos].GetComponent<ObjectsMenuButton>().uses == 0) indexBotonSeleccionadoObjetos--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                indexBotonSeleccionadoObjetos++;
                if (indexBotonSeleccionadoObjetos >= botonesObjetos.Length) indexBotonSeleccionadoObjetos = 0;
                if (botonesObjetos[indexBotonSeleccionadoObjetos].name != "Exit" && botonesObjetos[indexBotonSeleccionadoObjetos].GetComponent<ObjectsMenuButton>().uses == 0) indexBotonSeleccionadoObjetos++;
                ButtonNavigationDown();
            }

            // Resalta el botón seleccionado y desactiva los otros
            for (int i = 0; i < botonesObjetos.Length; i++)
            {
                if (i == indexBotonSeleccionadoObjetos)
                {
                    // Resalta el botón seleccionado
                    //botonesObjetos[i].GetComponent<Button>().interactable = true;
                    botonesObjetos[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    // Desactiva los otros botones
                    botonesObjetos[i].GetComponent<Button>().interactable = false;
                }
            }
            // Cambia el botón seleccionado y realiza una acción cuando se presiona la barra espaciadora
            if (Input.GetKeyDown(KeyCode.Space))
            {
                botonesObjetos[indexBotonSeleccionadoObjetos].GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}