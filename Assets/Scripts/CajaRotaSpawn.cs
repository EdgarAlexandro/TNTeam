using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    public float probability;
}

public class CajaRotaSpawn : MonoBehaviour
{
    public List<SpawnableObject> objectsPrefabs = new();
    public Transform spawnPoint;
    public GameObject joker;
    private DestructionManager dm;

    void Start()
    {
        dm = DestructionManager.Instance;
        spawnPoint = gameObject.transform;
        string boxIdentifier = gameObject.name;

        // Check if box is supposed to be destroyed
        if (dm.IsDestroyed(boxIdentifier))
        {
            Destroy(gameObject);
        }
    }

    // Spawn an object from the list of prefabs
    public void SpawnObject()
    {
        if (objectsPrefabs.Count > 0 && spawnPoint != null)
        {
            float totalProbability = 0f;
            foreach (SpawnableObject spawnableObject in objectsPrefabs)
            {
                totalProbability += spawnableObject.probability;
            }
            float randomValue = Random.Range(0f, totalProbability);
            foreach (SpawnableObject spawnableObject in objectsPrefabs)
            {
                if (randomValue <= spawnableObject.probability)
                {
                    Instantiate(spawnableObject.prefab, spawnPoint.position, Quaternion.identity);
                    break; 
                }
                randomValue -= spawnableObject.probability;
            }
        }
    }

    // Spawn Joker character
    public void SpawnJoker()
    {
        Instantiate(joker, spawnPoint.position, Quaternion.identity);
    }
}
