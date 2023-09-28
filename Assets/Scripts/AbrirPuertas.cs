using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuertas : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<UIController>().currentKeys >= (1))
            {
                Destroy(gameObject);
            }
        }
    }


}
