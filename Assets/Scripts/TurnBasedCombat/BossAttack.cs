/* Function: Boss attack's behaviour
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BossAttack : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public float damageMultiplier;
    public float playerDefense;
    public float force = 1.0f;
    public float damage = 5.0f;
    private Vector3 playerPosition;
    private PersistenceManager pm;
    private TurnBasedCombatManager tbc;
    private TurnBasedCombatPlayersHealth tbcPH;
    private TurnBasedCombatPlayerDeath tbcPD;
    private GameObject healthBars;
    //public GameObject dodgeMessage;
    //public GameObject turnBasedCombatCanvas;
    public bool isHittingShield = false;
    private GameObject playerObject;
    private Rigidbody2D rigidBody;
    public bool isMoving = false;    


    void Start() {
        damageMultiplier = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>().BossAttackMultiplier;
        playerDefense = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>().playerDefenseMultiplier;
        rigidBody = GetComponent<Rigidbody2D>();
        pm = PersistenceManager.Instance;
        tbc = TurnBasedCombatManager.Instance;
        //tbcPD = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatPlayerDeath>();
        healthBars = GameObject.Find("HealthBars");
        //dodgeMessage = GameObject.Find("DodgeMsg");
        tbcPH = healthBars.GetComponent<TurnBasedCombatPlayersHealth>();
    }
    // When the object is instantiated on the network, get the sender's info and point the attack towards them.
    public void OnPhotonInstantiate(PhotonMessageInfo info){
        if (info.Sender.IsLocal){
            playerObject = info.Sender.TagObject as GameObject; // Get the sender's tagObject that was set in CharacterAssignation as a gameobject.
            playerPosition = playerObject.transform.position; // Get targeted player's position.
            photonView.RPC("SyncronizePosition", RpcTarget.All, playerPosition); // Call PunRPC so the targeted player's position is the same for all clients.
            photonView.RPC("SyncronizeAttackStatus", RpcTarget.All, true); // Call PunRPC so the targeted player's position is the same for all clients.
        }
    }

    [PunRPC]
    public void SyncronizeAttackStatus(bool status)
    {
        isMoving = status;
    }

    //PunRPC to set the targeted player's position for all clients.
    [PunRPC]
    public void SyncronizePosition(Vector3 attackNewPosition){
        playerPosition = attackNewPosition;
    }

    void Update(){
        if (isMoving){// If attack hasn't been destroyed call PunRPC so attack moves towards the targeted player for all clients.
            MoveTowardsPlayer();
        }
    }
    // Attack moves towards the targeted player for all clients.
    public void MoveTowardsPlayer() {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        rigidBody.AddForce((playerPosition - transform.position).normalized * force);
    }
    // Detect when the attack enters a player's trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
             
            Debug.Log("Triggered");
            if (photonView.IsMine)
            {  // If the player is local only them take damage.
                if (pm.CurrentHealth - (int)damage > 0)
                {
                    pm.CurrentHealth -= (int)((damage*damageMultiplier)/playerDefense); // toma el ataque, lo multiplica por el multiplicador de ataque del jefe, y lo divide entre el multiplicador de defensa del jugador

                }
                else
                {
                    pm.CurrentHealth -= (int)damage;
                    other.gameObject.GetComponent<TurnBasedCombatPlayerDeath>().OnPlayerDeath(other.gameObject);
                }

                StartCoroutine(AlternateColors(other.gameObject.name)); // Coroutine to display that the player took damage by changing its colors.

                if (PhotonNetwork.IsMasterClient)
                { // If attacked player is the master client, update the player 1's health bar with current health.                 
                    tbc.photonView.RPC("SyncronizeP1Health", RpcTarget.All, pm.CurrentHealth);
                    tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                }
                else
                {// If attacked player is not the master client, update the player 2's health bar with current health.
                    tbc.photonView.RPC("SyncronizeP2Health", RpcTarget.All, pm.CurrentHealth);
                    tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                }
                //gameObject.layer = LayerMask.NameToLayer("NoCollision");
                //StartCoroutine("DestroyObject"); // Coroutine to destroy the attack's gameObject, it waits until alternate colors finishes its execution
            }
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            /*PhotonView photonView = other.transform.parent.gameObject.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                Debug.Log("Other player is local");
                playerObject.GetComponent<TurnBasedCombatPlayerControl>().canBlock = false;
                tbc.EndTurn();
                PhotonNetwork.Destroy(gameObject);
            }*/
        }
    }

    private void OnBecameInvisible()
    {
        // The object became invisible, destroy it.
        DestroyObject();
    }

    // Remote procedure call to change player's color
    [PunRPC]
    public void ChangeColor(string player){
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
    }
    // Remote procedure call to restore player's color
    [PunRPC]
    public void ReturnColor(string player){
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(string player){
        photonView.RPC("ChangeColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
        photonView.RPC("ReturnColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
    }
    // Coroutine to destroy the attack's gameObject,
    public void DestroyObject(){
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            playerObject.GetComponent<TurnBasedCombatPlayerControl>().canBlock = false;
            tbc.EndTurn();
        }  
    }
}
