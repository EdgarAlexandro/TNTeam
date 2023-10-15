using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BoxInstantiate : MonoBehaviourPunCallbacks
{
    private DestructionManager dm;
    private Transform spawnPoint;
    public string cajaGameObjString;
    private GameObject caja;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            dm = DestructionManager.Instance;
            spawnPoint = gameObject.transform;
            string boxIdentifier = gameObject.name;

            // Check if box is supposed to be destroyed
            if (!dm.IsDestroyed(boxIdentifier))
            {
                //caja = PhotonNetwork.Instantiate(cajaGameObjString, spawnPoint.position, Quaternion.identity);
                caja = PhotonNetwork.InstantiateRoomObject(cajaGameObjString, spawnPoint.position, Quaternion.identity);
                caja.name = boxIdentifier;
            }
        }
    }
}
