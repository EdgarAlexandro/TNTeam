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

    public void OnPlayerDeath()
    {
        Debug.Log("PlayerDeath");
        animator.SetBool("Muerte", true);
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
