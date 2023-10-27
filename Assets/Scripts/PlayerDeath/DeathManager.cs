using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathManager : MonoBehaviourPunCallbacks
{
    private static DeathManager instance;
    public static DeathManager Instance { get { return instance; } }

    public GameObject DeadPlayerPrefab;

    // Singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SpawnDeadPlayerPrefab(Vector2 spawnPosition)
    {
        PhotonNetwork.Instantiate(DeadPlayerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
