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

    public int bossHealth = 10;
    private int bossHealthMaxValue = 10;

    void Start()
    {
        photonView.RPC("SetHealthBarValues", RpcTarget.All);
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        bossHealth -= damage;
        photonView.RPC("SetHealthBarCurrentValue", RpcTarget.All);
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
            Destroy(gameObject);
        }
    }
}
