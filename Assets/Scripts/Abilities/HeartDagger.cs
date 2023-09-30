using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartDagger : MonoBehaviour
{
    float probabilidadFuncionA;
    float randomValue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (other.CompareTag("Caja"))
        {
            List<string> availableScenes = JokerSpawn.Instance.availableScenes;
            if (availableScenes.Contains(currentSceneName))
            {
                probabilidadFuncionA = 0.8f;
                Debug.Log("La probabilidad de aparicion es " + probabilidadFuncionA);
                Debug.Log(availableScenes.Contains(currentSceneName));
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
                }
            }
            else
            {
                other.GetComponent<CajaRotaSpawn>().SpawnObject();
            }

            Destroy(other.gameObject);
            //this.velocity = projDirection * speed;
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
    
    private void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
