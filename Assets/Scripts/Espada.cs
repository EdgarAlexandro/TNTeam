using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
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
    }
}