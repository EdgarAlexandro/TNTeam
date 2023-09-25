using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDagger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Caja"))
        {
            Destroy(other.gameObject);
            //this.velocity = projDirection * speed;
        }
    }
    
    private void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
