using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaRotaSpawn : MonoBehaviour
{
    public List<GameObject> objectsPrefabs = new List<GameObject>();
    public Transform spawnPoint;

    void Start()
    {
        spawnPoint = gameObject.transform;
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject obj in objectsWithTag)
        {
            objectsPrefabs.Add(obj);
        }
    }

    private void OnDestroy()
    {
        if (objectsPrefabs.Count > 0 && spawnPoint != null)
        {
            GameObject selectedPrefab = objectsPrefabs[Random.Range(0, objectsPrefabs.Count)];
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
