/* Function: Display the players health in their corresponding health bar
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;

public class TurnBasedCombatPlayersHealth : MonoBehaviourPunCallbacks
{
    public Slider p1HealthBar;
    public Slider p2HealthBar;

    private TurnBasedCombatManager tbc;
    private PersistenceManager pm;
    public List<GameObject> playersGameObject = new List<GameObject>();
    public List<PlayerInNetwork> players;

    // Start is called before the first frame update
    void Start(){
        tbc = TurnBasedCombatManager.Instance;
        pm = PersistenceManager.Instance;
        players = tbc.players;
        photonView.RPC("SetHealthBarValues", RpcTarget.All);
    }
    // Sets health values at the start of the turn based combat.
    [PunRPC]
    public void SetHealthBarValues(){
        if (PhotonNetwork.IsMasterClient) // If player is master, with a PunRPC syncronize player 1's health bar values for all clients.
        {
            p1HealthBar.maxValue = pm.MaxHealth;
            p1HealthBar.value = pm.CurrentHealth;
            photonView.RPC("SyncronizeP1HealthBarValue", RpcTarget.All, p1HealthBar.maxValue, p1HealthBar.value);
        }
        else { //If player is not master, with a PunRPC syncronize player 2's health bar values for all clients.
            p2HealthBar.maxValue = pm.MaxHealth;
            p2HealthBar.value = pm.CurrentHealth;
            photonView.RPC("SyncronizeP2HealthBarValue", RpcTarget.All, p2HealthBar.maxValue, p2HealthBar.value);
        }
    }
    // PunRPC to syncronize player 1's health bar values for all clients.
    [PunRPC]
    public void SyncronizeP1HealthBarValue(float maxValue, float value){
        p1HealthBar.maxValue = maxValue;
        p1HealthBar.value = value;
    }
    // PunRPC to syncronize player 1's health bar values for all clients.
    [PunRPC]
    public void SyncronizeP2HealthBarValue(float maxValue, float value){
        p2HealthBar.maxValue = maxValue;
        p2HealthBar.value = value;
    }
    // PunRPC to syncronize player 1's health bar current health value when it receives a modification for all clients.
    [PunRPC]
    public void SyncronizeP1HealthBarCurrentValue(int value){
        p1HealthBar.value = value;
    }
    // PunRPC to syncronize player 2's health bar current health value when it receives a modification for all clients.
    [PunRPC]
    public void SyncronizeP2HealthBarCurrentValue(int value){
        p2HealthBar.value = value;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
