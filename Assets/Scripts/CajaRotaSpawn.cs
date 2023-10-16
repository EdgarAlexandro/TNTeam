/* Function: spawn objects or Joker when box is destroyed given the probability
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Class that stores the info of the spawnable object (defined in unity editor)
[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab = null;
    public float probability = 0.0f;
    public string itemName = "";
}

public class CajaRotaSpawn : MonoBehaviourPunCallbacks
{
    // List of available objects (defined in unity editor)
    public List<SpawnableObject> objectsPrefabs = new();
    public string joker = "";
    public Transform spawnPoint = null;
    private bool objectHasSpawned = false;

    void Start()
    {
        spawnPoint = gameObject.transform;
    }

    // Remote Procedure that destroys the box for all players
    [PunRPC]
    public void DestroyBox()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    // Spawn an object from the list of prefabs
    public void SpawnObject()
    {
        if (objectsPrefabs.Count > 0 && spawnPoint != null)
        {
            // The sum of the probability of all objects
            float totalProbability = 0f;
            foreach (SpawnableObject spawnableObject in objectsPrefabs)
            {
                totalProbability += spawnableObject.probability;
            }
            // If the random value is less than the probability of the object, it spawns the object
            float randomValue = Random.Range(0f, totalProbability);
            foreach (SpawnableObject spawnableObject in objectsPrefabs)
            {
                if (randomValue <= spawnableObject.probability && !objectHasSpawned)
                {
                    PhotonNetwork.Instantiate(spawnableObject.itemName, spawnPoint.position, Quaternion.identity);
                    objectHasSpawned = true;
                }
                randomValue -= spawnableObject.probability;
            }
        }
    }

    // Spawn Joker character
    public void SpawnJoker()
    {
        PhotonNetwork.Instantiate(joker, spawnPoint.position, Quaternion.identity);
    }
}
