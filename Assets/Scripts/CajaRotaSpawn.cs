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
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Object");
        GameObject[] orbsWithTag = GameObject.FindGameObjectsWithTag("Orbe");

        foreach (GameObject obj in objectsWithTag)
        {
            objectsPrefabs.Add(obj);
        }

        foreach (GameObject orb in orbsWithTag)
        {
            objectsPrefabs.Add(orb);
        }
    }

    public void SpawnObject()
    {
        if (objectsPrefabs.Count > 0 && spawnPoint != null)
        {
            GameObject selectedPrefab = objectsPrefabs[Random.Range(0, objectsPrefabs.Count)];
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void SpawnJoker()
    {
        Instantiate(joker, spawnPoint.position, Quaternion.identity);
    }
}