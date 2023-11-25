using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCombatPlayerDeath : MonoBehaviourPunCallbacks
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPlayerDeath(GameObject player)
    {
        Debug.Log("PlayerDeath");
        animator.SetBool("Muerte", true);
        photonView.RPC("OtherPlayerDeathAnimation", RpcTarget.All, player.name);
        photonView.RPC("UpdateAlivePlayers", RpcTarget.All);
    }

    [PunRPC]
    public void OtherPlayerDeathAnimation(string playerName)
    {
        GameObject player = GameObject.Find(playerName);
        player.GetComponent<Animator>().SetBool("Muerte", true);
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
