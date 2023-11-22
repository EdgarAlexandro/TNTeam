/* Function: controls the behaviour of the object/item the player´s weapon hits
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Espada : MonoBehaviourPunCallbacks
{
    public int damage = 1;
    public float knockback = 5.0f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the object the sword hit is a box
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking") && gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            StartCoroutine(CajaDestruida(other.gameObject,currentSceneName));
        }

        // Check if the object the sword hit is an enemy
        if (other.CompareTag("Enemy") && this.GetComponentInParent<Animator>().GetBool("isAttacking") && gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            other.gameObject.TryGetComponent(out EnemyAi enemyComponent);
            if (transform.parent.TryGetComponent(out ChargeAttack chargeA))
            {
                if (chargeA.isChargeAttacking)
                {
                    enemyComponent.OnHit(chargeA.chargeDmg, knockbackDirection, knockback * 2);
                }
            }
            else
            {
                // Deals damage to the enemy and applys a knockback to it
                enemyComponent.OnHit(damage, knockbackDirection, knockback);
            }
        }

        // Check if the object the sword hit is a spawner
        if (other.CompareTag("Spawner") && this.GetComponentInParent<Animator>().GetBool("isAttacking") && gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            other.gameObject.TryGetComponent(out SpawnerScript spawnerComponent);
            // Deals damage to the spawner
            spawnerComponent.OnHit(damage);
        }
    }

    IEnumerator CajaDestruida(GameObject Caja, string escena)
    {
        Caja.GetComponent<Animator>().Play("Caja destruida");
        yield return new WaitForSeconds(0.40f);
    }
}