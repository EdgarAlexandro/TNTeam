using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LeverSpawn : MonoBehaviourPunCallbacks
{
    private DestructionManager dm = null;
    private Transform spawnPoint = null;
    public string leverGameObjString = "";
    private GameObject lever = null;

    void Start()
    {
        //the levers are created on the host's side
        if (PhotonNetwork.IsMasterClient)
        {
            dm = DestructionManager.Instance;
            spawnPoint = gameObject.transform;
            //the identifier will be the name of the spawn point gameobject
            string spawnerIdentifier = gameObject.name;

            // Check if lever is supposed to be destroyed
            if (!dm.IsDestroyed(spawnerIdentifier))
            {
                //Instantiate object in Photon Network
                lever = PhotonNetwork.InstantiateRoomObject(leverGameObjString, spawnPoint.position, Quaternion.identity);
                //Name the lever after the spawn point gameobject (different names for each scene)
                lever.name = spawnerIdentifier;
            }
        }
    }
}
