using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WaterandLavaGrid waterGrid = FindObjectOfType<WaterandLavaGrid>();
            waterGrid.SetPlayerOnBridge(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WaterandLavaGrid waterGrid = FindObjectOfType<WaterandLavaGrid>();
            waterGrid.SetPlayerOnBridge(false);
        }
    }
}
