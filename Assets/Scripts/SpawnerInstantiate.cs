using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnerInstantiate : MonoBehaviourPunCallbacks
{
    private DestructionManager dm;
    private Transform spawnPoint;
    public string spawnerGameObjString;
    private GameObject spawner;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            dm = DestructionManager.Instance;
            spawnPoint = gameObject.transform;
            string spawnerIdentifier = gameObject.name;

            // Check if spawner is supposed to be destroyed
            if (!dm.IsDestroyed(spawnerIdentifier))
            {
                spawner = PhotonNetwork.InstantiateRoomObject(spawnerGameObjString, spawnPoint.position, Quaternion.identity);
                spawner.name = spawnerIdentifier;

            }
        }
    }
}