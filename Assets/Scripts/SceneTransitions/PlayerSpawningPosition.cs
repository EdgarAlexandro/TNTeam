/* Function: Change player position after changing scene
   Author: Daniel Degollado Rodríguez
   Modification date: 22/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawningPosition : MonoBehaviourPunCallbacks{
    public GameObject[] spawnPoints;
    public GameObject[] players;
 
    void Start(){
        if (PhotonNetwork.IsConnectedAndReady){
            GetPlayersInScene();
            PositionPlayers();
        }
        else{
            Debug.LogWarning("Photon not connected and ready.");
        }
    }
    // Find all players in scene.
    void GetPlayersInScene(){
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    // Change player's position to that of a spawn point.
    void PositionPlayers(){
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (GameObject spawnPoint in spawnPoints){
            foreach (GameObject player in players){   
                // Dead players stay in the same position.
                if(player.GetComponent<UIController>().isDead == false) {
                    float randomYOffset = Random.Range(-0.5f, 0.5f);
                    player.transform.position = spawnPoint.transform.position + new Vector3(0f, randomYOffset, 0f); 
                } 
            }
        }
    }
}
