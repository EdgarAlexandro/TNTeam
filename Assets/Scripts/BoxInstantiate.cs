/* Function: spawn boxes upon entering a scene if they have'nt been destroyed
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoxInstantiate : MonoBehaviourPunCallbacks
{
    // GameObject (Point) where the box will be created
    private Transform spawnPoint = null;
    public string cajaGameObjString = "";
    private GameObject caja = null;
    private DestructionManager dm = null;

    void Start()
    {
        //the boxes are created on the host's side
        if (PhotonNetwork.IsMasterClient)
        {
            dm = DestructionManager.Instance;
            spawnPoint = gameObject.transform;
            //the identifier will be the name of the spawn point gameobject
            string boxIdentifier = gameObject.name;

            // Check if box isnt destroyed
            if (!dm.IsDestroyed(boxIdentifier))
            {
                //Instantiate object in Photon Network
                caja = PhotonNetwork.Instantiate(cajaGameObjString, spawnPoint.position, Quaternion.identity);
                //Name the box after the spawn point gameobject (different names for each scene)
                caja.name = boxIdentifier;
            }
        }
    }
}
