using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbeDeteccion : MonoBehaviour
{
    public Rigidbody2D rig;
    private Transform jugador;
    public float velocidadMovimiento = 0.1f;

    void Start()
    {
        jugador = null;
    }

    void Update()
    {
        if (jugador != null)
        {
            Vector2 nuevaPosicion = Vector2.Lerp(transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
            rig.MovePosition(nuevaPosicion);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = null;
        }
    }
}
