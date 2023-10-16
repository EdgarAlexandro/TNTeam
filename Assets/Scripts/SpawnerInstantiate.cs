/* Function: spawn enemy spawners upon entering a scene if they have'nt been destroyed
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerInstantiate : MonoBehaviourPunCallbacks
{
    private DestructionManager dm = null;
    private Transform spawnPoint = null;
    public string spawnerGameObjString = "";
    private GameObject spawner = null;

    void Start()
    {
        //the spawners are created on the host's side
        if (PhotonNetwork.IsMasterClient)
        {
            dm = DestructionManager.Instance;
            spawnPoint = gameObject.transform;
            //the identifier will be the name of the spawn point gameobject
            string spawnerIdentifier = gameObject.name;

            // Check if spawner is supposed to be destroyed
            if (!dm.IsDestroyed(spawnerIdentifier))
            {
                //Instantiate object in Photon Network
                spawner = PhotonNetwork.InstantiateRoomObject(spawnerGameObjString, spawnPoint.position, Quaternion.identity);
                //Name the spawner after the spawn point gameobject (different names for each scene)
                spawner.name = spawnerIdentifier;
            }
        }
    }
}