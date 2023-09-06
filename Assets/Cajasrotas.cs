using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cajasrotas : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Caja"))
        {
            Destroy(other.gameObject);
        }
    }
}

