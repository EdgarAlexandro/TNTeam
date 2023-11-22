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
    private MusicSFXManager musicSFXManager;
    float probabilidadFuncionA = 0.0f;
    float randomValue = 0.0f;
    private DestructionManager dm = null;
    private JokerSpawn jk = null;

    void Start()
    {
        spawnPoint = gameObject.transform;
        musicSFXManager = MusicSFXManager.Instance;
        dm = DestructionManager.Instance;
        jk = JokerSpawn.Instance;
    }

    // Remote Procedure that destroys the box for all players
    [PunRPC]
    public void DestroyBox()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Caja_Rota);
            string boxIdentifier = gameObject.name;
            // Marks the box as destroyed
            dm.MarkAsDestroyed(boxIdentifier);
            PhotonNetwork.Destroy(gameObject);
        }
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
        musicSFXManager.PlaySFX(MusicSFXManager.Instance.Joker_Sound);
    }

    //Chooses to spawn a box or the joker
    public void boxDestructionAux(string currentSceneName)
    {
        List<string> availableScenes = jk.availableScenes;
        

        // Check if the current scene was selected as one of the scenes available for Joker spawning
        if (availableScenes.Contains(currentSceneName))
        {
            probabilidadFuncionA = 0.8f;
            randomValue = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;

            // If the random value is less than the defined probability value, it spawns an object
            if (randomValue < probabilidadFuncionA)
            {
                SpawnObject();
            }
            // If it is more than the defined probability value, it spawns the Joker
            else
            {
                SpawnJoker();
                jk.RemoveScene(currentSceneName);
            }
        }
        // If it's not available, it spawns an object
        else
        {
            SpawnObject();
        }

        
        photonView.RPC("DestroyBox", RpcTarget.All);
    }
}
