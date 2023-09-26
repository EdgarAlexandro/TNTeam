using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 5f;

    float probabilidadFuncionA;
    float randomValue;

    void Start()
    {
        probabilidadFuncionA = 0.8f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            randomValue = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;
            if (randomValue < probabilidadFuncionA)
            {
                other.GetComponent<CajaRotaSpawn>().SpawnObject();
            }
            else
            {
                other.GetComponent<CajaRotaSpawn>().SpawnJoker();
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy" && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            other.gameObject.TryGetComponent<EnemyAi>(out EnemyAi enemyComponent);
            enemyComponent.OnHit(damage, knockbackDirection, knockback);
        }

        if (other.gameObject.tag == "Spawner" && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            other.gameObject.TryGetComponent<SpawnerScript>(out SpawnerScript spawnerComponent);
            spawnerComponent.OnHit(damage);
        }
    }
}