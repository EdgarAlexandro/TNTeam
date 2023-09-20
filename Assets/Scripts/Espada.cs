using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            Destroy(other.gameObject);
        }
    }
}
