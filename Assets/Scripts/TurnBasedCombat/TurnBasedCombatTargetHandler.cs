/* Function: Spawn attack depending on which player is targeted at the moment
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCombatTargetHandler : MonoBehaviourPunCallbacks
{

    public TurnBasedCombatPlayersHealth tbcPH;
    public TurnBasedCombatManager tbc;
    private PersistenceManager pm;
    public List<PlayerInNetwork> players;
    Photon.Realtime.Player player;
    public GameObject bossAttack;
    public GameObject boss;
    public GameObject dodgeMessage;
    //public GameObject chracterPrefabs;

    // Start is called before the first frame update
    void Start(){
        tbc = TurnBasedCombatManager.Instance;
        pm = PersistenceManager.Instance;

        //chracterPrefabs = tbc.prefabs;
        StartCoroutine("AssignPlayers");
        //photonView.RPC("InitializePlayers", RpcTarget.All);
    }

    IEnumerator AssignPlayers(){
        yield return new WaitForSeconds(0.6f);
        players = tbc.players;
    }

    // Update is called once per frame
    void Update(){
        
    }

    public static TurnBasedCombatTargetHandler Instance { get; private set; }

    private void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // PunRPC that checks if the targeted player is local and instantiate the attack. Takes player's index as a parameter.
    [PunRPC]
    public void TakeDamageTBC(int playerIndex){
        Debug.Log("Im taking damage: " + players[playerIndex].Name);
        if (players[playerIndex].IsLocal)
        {
            //GameObject localPlayerObject = players[playerIndex].tagObject as GameObject;
            PhotonNetwork.Instantiate(bossAttack.name, boss.transform.position, Quaternion.identity);
            //StartCoroutine("DodgeMessage");

            /*if (pm.CurrentHealth > 0)
            {
                pm.CurrentHealth -= damage;
                GameObject localPlayerObject = players[playerIndex].tagObject as GameObject;
                StartCoroutine(AlternateColors(localPlayerObject.name));
            }
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonView photonView = tbcPH.GetComponent<PhotonView>();
                photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else{
                PhotonView photonView = tbcPH.GetComponent<PhotonView>();
                photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }*/
        }
        /*if (pm.CurrentHealth > 0)
        {
            pm.CurrentHealth -= damage;
        }*/
        /*healthBar.SetHealth(pm.CurrentHealth);
        if (pm.CurrentHealth == 0)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
                NetworkManager.instance.LoadScene("LoseScene");
            }
            else
            {
                PlayerDied();
                if (GameController.AlivePlayers == 0)
                {
                    NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "LoseScene");
                }
            }
        }
        else
        {
            StartCoroutine(AlternateColors(player));
        }*/
    }

    public IEnumerator DodgeMessage(){
        dodgeMessage.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        dodgeMessage.SetActive(false);
    }
}
