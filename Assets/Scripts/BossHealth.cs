using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BossHealth : MonoBehaviourPunCallbacks
{ 
    public Slider bossHealthBar;
    int randomIndex;
    System.Random random;
    Animator animator;

    public int bossHealth = 10;
    private int bossHealthMaxValue = 10;

    void Start()
    {
        animator = GetComponent<Animator>();
        photonView.RPC("SetHealthBarValues", RpcTarget.All);
    }

    public void TakeDamage(int damage)
    {
        bossHealth -= damage;
        photonView.RPC("SyncronizeBossHealth", RpcTarget.All, bossHealth);
        photonView.RPC("SetHealthBarCurrentValue", RpcTarget.All);
    }

    [PunRPC]
    private void SyncronizeBossHealth(int currentBossHealth)
    {
        bossHealth = currentBossHealth; 
    }

    [PunRPC]
    private void SetHealthBarValues()
    {
        bossHealthBar.maxValue = bossHealthMaxValue;
        bossHealthBar.value = bossHealth;   
    }

    [PunRPC]
    private void SetHealthBarCurrentValue()
    {
        bossHealthBar.value = bossHealth;
    }

    /*[PunRPC]
    public void SelectTarget(List<PlayerInNetwork> players)
    {
        if (PhotonNetwork.IsMasterClient){
            random = new System.Random();
            randomIndex = random.Next(players.Count);
            photonView.RPC("SyncronizeRandomIndex", RpcTarget.All, randomIndex);
        }
        tbcUI.photonView.RPC("TakeDamageTBC", RpcTarget.All, 5, players[randomIndex]);
    }

    [PunRPC]
    private void SyncronizeRandomIndex(int index)
    {
        randomIndex = index;
    }*/

    // Update is called once per frame
    void Update()
    {
       if( bossHealth <= 0)
        {
            animator.SetBool("Muerte", true);
            //Destroy(gameObject);
        }
    }
}
