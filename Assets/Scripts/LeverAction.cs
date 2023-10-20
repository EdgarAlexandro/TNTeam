using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            PersistenceManager.Instance.LeverCounter++;
            Destroy(this.gameObject);
        }
    }
}
