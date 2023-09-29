using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaRotaSpawn : MonoBehaviour
{
    public List<GameObject> objectsPrefabs = new List<GameObject>();
    public Transform spawnPoint;
    public GameObject joker;

    void Start()
    {
        spawnPoint = gameObject.transform;
    }

    public void SpawnObject()
    {
        if (objectsPrefabs.Count > 0 && spawnPoint != null)
        {
            GameObject selectedPrefab = objectsPrefabs[0];
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void SpawnJoker()
    {
        Instantiate(joker, spawnPoint.position, Quaternion.identity);
    }
}