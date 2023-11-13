/* Function: Sets the sender's tagObject to this gameObject's
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAssignation : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject playerCharacter;
    // When the object is instantiated on the network, get the sender's info and set it's tagObject to this gameObject's if its the local client
    public void OnPhotonInstantiate(PhotonMessageInfo info){
        if (info.Sender.IsLocal){
            Debug.Log(gameObject.name + "Belongs to: " + info.Sender.NickName);
            info.Sender.TagObject = this.gameObject;
        }
    }
}
