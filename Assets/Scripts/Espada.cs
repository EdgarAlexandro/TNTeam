using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Espada : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 5f;

    float probabilidadFuncionA;
    float randomValue;

    void Start()
    {
        //probabilidadFuncionA = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            List<string> availableScenes = JokerSpawn.Instance.availableScenes;
            Debug.Log(availableScenes);
            //if (other.GetComponentInParent<JokerSpawn>().IsSceneAvailable(currentSceneName)){
            if(availableScenes.Contains(currentSceneName))
            { 
                probabilidadFuncionA = 0.8f;
                Debug.Log("La probabilidad de aparicion es " + probabilidadFuncionA);
                Debug.Log(availableScenes.Contains(currentSceneName));
            }
            else
            {
                probabilidadFuncionA = 1.1f;
                Debug.Log("La probabilidad de aparicion es " + probabilidadFuncionA);
                Debug.Log(availableScenes.Contains(currentSceneName));
            }

            randomValue = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;
            Debug.Log(randomValue);
            if (randomValue < probabilidadFuncionA)
            {
                other.GetComponent<CajaRotaSpawn>().SpawnObject();
            }
            else
            {
                other.GetComponent<CajaRotaSpawn>().SpawnJoker();
                JokerSpawn.Instance.RemoveScene(currentSceneName);
                Debug.Log(availableScenes);
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