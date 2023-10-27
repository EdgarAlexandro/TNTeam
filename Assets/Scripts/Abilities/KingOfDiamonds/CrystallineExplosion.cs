using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallineExplosion : MonoBehaviour
{
    private MusicSFXManager musicSFXManager;
    void Start(){
        StartCoroutine(ExTimer());
        musicSFXManager = MusicSFXManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Caja"))
        {
            Destroy(other.gameObject);
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Explosion);
            //this.velocity = projDirection * speed;
        }
    }
    
    IEnumerator ExTimer(){
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
