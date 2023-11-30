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
    public List<Sprite> playerHealthBars = new List<Sprite>();
    public List<PlayerInNetwork> players;

    // Start is called before the first frame update
    void Start(){
        if (PhotonNetwork.OfflineMode)
        {
            SinglePlayerHealthBar();
        }
        playersGameObject = GameObject.FindGameObjectsWithTag("Player").ToList();
        tbc = TurnBasedCombatManager.Instance;
        pm = PersistenceManager.Instance;
        players = tbc.players;
        SetHealthBarValues();
        CheckCurrentCharacter();    
    }

    void SinglePlayerHealthBar()
    {
        p2HealthBar.gameObject.SetActive(false);
        p1HealthBar.gameObject.transform.position = new Vector3(-6.3f, 1.5f, 0f);
    }

    private void CheckCurrentCharacter()
    {
        foreach (GameObject player in playersGameObject)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                GetSelectedCharacterHealthBarSprite(player);
            }
        }
    }

    private void GetSelectedCharacterHealthBarSprite(GameObject player)
    {
        string playerName = player.name.Replace("(Clone)", "");
        foreach (Sprite healthBar in playerHealthBars)
        {
            if (healthBar.name.StartsWith(playerName))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    p1HealthBar.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = healthBar;
                    photonView.RPC("SyncronizeP1HealthBarSprite", RpcTarget.All, healthBar.name);
                }
                else
                {
                    p2HealthBar.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = healthBar;
                    photonView.RPC("SyncronizeP2HealthBarSprite", RpcTarget.All, healthBar.name);
                }
            }
        }
    }

    [PunRPC]
    public void SyncronizeP1HealthBarSprite(string healthBarName)
    {
        foreach (Sprite healthBar in playerHealthBars)
        {
            if(healthBar.name == healthBarName)
            {
                p1HealthBar.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = healthBar;
            }
        }
    }

    [PunRPC]
    public void SyncronizeP2HealthBarSprite(string healthBarName)
    {
        foreach (Sprite healthBar in playerHealthBars)
        {
            if (healthBar.name == healthBarName)
            {
                p2HealthBar.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = healthBar;
            }
        }
    }

    // Sets health values at the start of the turn based combat.
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
