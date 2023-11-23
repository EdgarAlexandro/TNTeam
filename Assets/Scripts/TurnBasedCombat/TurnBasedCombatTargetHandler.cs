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
    PlayerInNetwork currentTarget;
    public GameObject bossAttack;
    public GameObject boss;
    public GameObject bossAttackSpawn;
    //public GameObject dodgeMessage;
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
    }
    // PunRPC that checks if the targeted player is local and instantiate the attack. Takes player's index as a parameter.
    [PunRPC]
    public void TakeDamageTBC(int playerIndex) {
        Debug.Log("Im taking damage: " + players[playerIndex].Name);
        currentTarget = players[playerIndex];
        if (currentTarget.IsLocal)
        {
            Debug.Log(players[playerIndex].Name + " is local");
            if (pm.CurrentHealth > 0)
            {
                GameObject targetedPlayerGameObject = players[playerIndex].tagObject as GameObject;
                targetedPlayerGameObject.GetComponent<TurnBasedCombatPlayerControl>().canBlock = true;
                photonView.RPC("AttackInitializing", RpcTarget.All);
            }
            else
            {
                if (!PhotonNetwork.OfflineMode)
                {
                    tbc.photonView.RPC("SelectTarget", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    public void AttackInitializing()
    {
        boss.GetComponent<BossAnimations>().Attack();
    }

    public void SpawnAttack()
    {
        if (currentTarget.IsLocal)
        {
            PhotonNetwork.Instantiate(bossAttack.name, bossAttackSpawn.transform.position, Quaternion.identity);
        }
    }

    /*public IEnumerator DodgeMessage(){
        dodgeMessage.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        dodgeMessage.SetActive(false);
    }*/
}
