using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCombatPlayerDeath : MonoBehaviourPunCallbacks
{
    public void OnPlayerDeath()
    {
        Debug.Log("PlayerDeath");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.black;
        photonView.RPC("UpdateAlivePlayers", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateAlivePlayers()
    {
        GameController.AlivePlayers--;
        Debug.Log(GameController.AlivePlayers);
        if (GameController.AlivePlayers == 0)
        {
            NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "LoseScene");
        }   
    }
}
