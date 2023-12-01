using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
            //Destroy(other.gameObject);
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<PhotonView>().RPC("DestroyEnemy", RpcTarget.MasterClient);
            }
            else
            {
                other.GetComponent<PhotonView>().RPC("DestroyBox", RpcTarget.MasterClient);
            }
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Explosion);
            //this.velocity = projDirection * speed;
        }
    }
    
    IEnumerator ExTimer(){
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
