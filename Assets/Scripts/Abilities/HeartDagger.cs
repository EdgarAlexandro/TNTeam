using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class HeartDagger : MonoBehaviourPunCallbacks
{
    float probabilidadFuncionA;
    float randomValue;

    [PunRPC]
    public void MoveDagger(Vector2 projDirection, float speed )
    {
        GetComponent<Rigidbody2D>().velocity = projDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (other.CompareTag("Caja"))
        {
            string boxIdentifier = other.gameObject.name;
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
            DestructionManager.Instance.MarkAsDestroyed(boxIdentifier);
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
