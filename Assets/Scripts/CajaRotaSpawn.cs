using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    public float probability;
    public string itemName;
}

public class CajaRotaSpawn : MonoBehaviourPunCallbacks
{
    public List<SpawnableObject> objectsPrefabs = new();
    public Transform spawnPoint;
    public GameObject joker;

    void Start()
    {
        spawnPoint = gameObject.transform;
    }

    [PunRPC]
    public void DestroyBox()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    // Spawn an object from the list of prefabs
    public void SpawnObject()
    {
        //photonView.RPC("spawnObjectPhoton", RpcTarget.All);
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
                    PhotonNetwork.Instantiate(spawnableObject.itemName, spawnPoint.position, Quaternion.identity);
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
