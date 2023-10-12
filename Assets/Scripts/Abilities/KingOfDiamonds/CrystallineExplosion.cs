using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallineExplosion : MonoBehaviour
{
    void Start(){
        StartCoroutine(ExTimer());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Caja"))
        {
            Destroy(other.gameObject);
            //this.velocity = projDirection * speed;
        }
    }
    
    IEnumerator ExTimer(){
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
