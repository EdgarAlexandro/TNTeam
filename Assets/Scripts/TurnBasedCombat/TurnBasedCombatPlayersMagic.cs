using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TurnBasedCombatPlayersMagic : MonoBehaviourPunCallbacks
{
    public Slider p1MagicBar;
    public Slider p2MagicBar;

    private PersistenceManager pm;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            SinglePlayerMagicBar();
        }
        pm = PersistenceManager.Instance;
        pm.CurrentMagic = 0;
        SetMagicBarValues();
    }

    void SinglePlayerMagicBar()
    {
        p2MagicBar.gameObject.SetActive(false);
        p1MagicBar.gameObject.transform.position = new Vector3(-6.3f, 1.5f, 0f);
    }


    // Sets health values at the start of the turn based combat.
    public void SetMagicBarValues()
    {
        if (PhotonNetwork.IsMasterClient) // If player is master, with a PunRPC syncronize player 1's health bar values for all clients.
        {
            p1MagicBar.maxValue = pm.MaxMagic;
            p1MagicBar.value = pm.CurrentMagic;
            photonView.RPC("SyncronizeP1MagicBarValue", RpcTarget.All, p1MagicBar.maxValue, p1MagicBar.value);
        }
        else
        { //If player is not master, with a PunRPC syncronize player 2's health bar values for all clients.
            p2MagicBar.maxValue = pm.MaxMagic;
            p2MagicBar.value = pm.CurrentMagic;
            photonView.RPC("SyncronizeP2MagicBarValue", RpcTarget.All, p2MagicBar.maxValue, p2MagicBar.value);
        }
    }
    // PunRPC to syncronize player 1's health bar values for all clients.
    [PunRPC]
    public void SyncronizeP1MagicBarValue(float maxValue, float value)
    {
        p1MagicBar.maxValue = maxValue;
        p1MagicBar.value = value;
    }
    // PunRPC to syncronize player 1's health bar values for all clients.
    [PunRPC]
    public void SyncronizeP2MagicBarValue(float maxValue, float value)
    {
        p2MagicBar.maxValue = maxValue;
        p2MagicBar.value = value;
    }
    // PunRPC to syncronize player 1's health bar current health value when it receives a modification for all clients.
    [PunRPC]
    public void SyncronizeP1MagicBarCurrentValue(int value)
    {
        p1MagicBar.value = value;
    }
    // PunRPC to syncronize player 2's health bar current health value when it receives a modification for all clients.
    [PunRPC]
    public void SyncronizeP2MagicBarCurrentValue(int value)
    {
        p2MagicBar.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
