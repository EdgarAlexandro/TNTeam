using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaRotaSpawn : MonoBehaviour
{
    public List<GameObject> objectsPrefabs = new List<GameObject>();
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
            GameObject selectedPrefab = objectsPrefabs[0];
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // Spawn Joker character
    public void SpawnJoker()
    {
        Instantiate(joker, spawnPoint.position, Quaternion.identity);
    }
}