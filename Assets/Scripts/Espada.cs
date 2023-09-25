using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 5f;

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
            enemyComponent.OnHit(damage, transform.right, knockback);
        }
    }
}
