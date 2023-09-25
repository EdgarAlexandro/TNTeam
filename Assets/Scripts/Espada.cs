using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            other.GetComponent<CajaRotaSpawn>().SpawnObject();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy" && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            other.gameObject.TryGetComponent<EnemyAi>(out EnemyAi enemyComponent);
            enemyComponent.TakeDamage(1);
        }


    }
}
