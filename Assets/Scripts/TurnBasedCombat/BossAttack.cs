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
    public float speed = 100f;
    public float damage = 5f;
    public float damageMultiplier;
    public float playerDefense;
    bool hasBeenDestroyed = false;
    private Vector3 playerPosition;
    private PersistenceManager pm;
    public TurnBasedCombatPlayersHealth tbcPH;
    private GameObject healthBars;
    public GameObject dodgeMessage;
    public GameObject turnBasedCombatCanvas;

    void Start(){
        damageMultiplier = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>().BossAttackMultiplier;
        playerDefense = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>().playerDefenseMultiplier;
        pm = PersistenceManager.Instance;
        healthBars = GameObject.Find("HealthBars");
        dodgeMessage = GameObject.Find("DodgeMsg");
        tbcPH = healthBars.GetComponent<TurnBasedCombatPlayersHealth>();
    }
    // When the object is instantiated on the network, get the sender's info and point the attack towards them.
    public void OnPhotonInstantiate(PhotonMessageInfo info){
        if (info.Sender.IsLocal){
            Debug.Log(info.Sender.NickName + " Created me");
            GameObject playerObject = info.Sender.TagObject as GameObject; // Get the sender's tagObject that was set in CharacterAssignation as a gameobject.
            Vector3 offset = playerObject.transform.position - transform.position; // Define offset.
            transform.rotation = Quaternion.LookRotation(Vector3.forward, offset); // Rotate attack so its facing the targeted player.
            Quaternion attackRotation = transform.rotation; // Store the rotation in a variable.
            photonView.RPC("SyncronizeRotation", RpcTarget.All, attackRotation); // Call PunRPC so the attack's rotation is the same for all clients.
            playerPosition = playerObject.transform.position; // Get targeted player's position.
            photonView.RPC("SyncronizePosition", RpcTarget.All, playerPosition); // Call PunRPC so the targeted player's position is the same for all clients.
        }
    }
    //PunRPC to set the attack's rotation for all clients.
    [PunRPC]
    public void SyncronizeRotation(Quaternion attackRotation){ 
        transform.rotation = attackRotation;
    }
    //PunRPC to set the targeted player's position for all clients.
    [PunRPC]
    public void SyncronizePosition(Vector3 attackNewPosition){
        playerPosition = attackNewPosition;
    }

    void Update(){
        if (!hasBeenDestroyed){// If attack hasn't been destroyed call PunRPC so attack moves towards the targeted player for all clients.

            photonView.RPC("MoveTowardsPlayer", RpcTarget.All);
        }
    }
    // PunRPC so attack moves towards the targeted player for all clients.
    [PunRPC]
    public void MoveTowardsPlayer() {
        transform.position = Vector3.Lerp(transform.position, playerPosition, speed * Time.deltaTime);
    }
    // Detect when the attack enters a player's trigger.
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            Debug.Log("Triggered");
            PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
            if (photonView.IsMine) {  // If the player is local only them take damage.
                if (pm.CurrentHealth > 0){
                    pm.CurrentHealth -= (int)((damage*damageMultiplier)/playerDefense);
                }                
                StartCoroutine(AlternateColors(other.gameObject.name)); // Coroutine to display that the player took damage by changing its colors.
                if (PhotonNetwork.IsMasterClient) { // If attacked player is the master client, update the player 1's health bar with current health.
                    //PhotonView photonViewTbcPH = tbcPH.GetComponent<PhotonView>();
                    tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                }
                else{// If attacked player is not the master client, update the player 2's health bar with current health.
                
                    PhotonView photonViewTbcPH = tbcPH.GetComponent<PhotonView>();
                    tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                }
                StartCoroutine("DestroyObject"); // Coroutine to destroy the attack's gameObject, it waits until alternate colors finishes its execution
            }
        }
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
        yield return new WaitForSeconds(0.2f);
        photonView.RPC("ReturnColor", RpcTarget.All, player);
    }
    // Coroutine to destroy the attack's gameObject,
    public IEnumerator DestroyObject(){
        yield return new WaitForSeconds(0.2f);
        PhotonNetwork.Destroy(gameObject);
        hasBeenDestroyed = true;
        //dodgeMessage.enabled = false;
    }

    /*[PunRPC]
    public void DestroyObjectInNetwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("DestroyObject");
        }
    }*/

    /*private void OnBecameInvisible()
    {
        PhotonNetwork.Destroy(gameObject);
        //dodgeMessage.enabled = false;
    }*/
}
