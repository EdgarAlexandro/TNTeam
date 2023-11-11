/* Function: Player class for each player in the network
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInNetwork
{
    public string Name { get; }
    public bool IsLocal { get; }
    public object tagObject { get; }

    public PlayerInNetwork(Photon.Realtime.Player photonPlayer){
        Name = photonPlayer.NickName;
        IsLocal = photonPlayer.IsLocal;
        tagObject = photonPlayer.TagObject;
    }
}
