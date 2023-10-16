/* Function: control the behaviour of the players spawn
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnController : MonoBehaviourPunCallbacks
{
    //Instancia (singleton)
    public static SpawnController instance = null;

    public Transform[] spawnPositions = null;
    public PlayerControl[] players = null;

    [SerializeField] private int playerInGame = 0;

    private void Awake()
    {
        instance = this;
        if (!GameController.instance.hasPlayersSpawned)
        {
            //If player is in offline mode, just spawns 1 character
            if (PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.Instantiate(MenuUIController.instance.p1, spawnPositions[0].position, Quaternion.identity);
            }
            else
            {
                players = new PlayerControl[PhotonNetwork.PlayerList.Length];
                photonView.RPC("InGame", RpcTarget.AllBuffered);
            }
            GameController.instance.hasPlayersSpawned = true;
        }

    }

    //Remote procedure call to spawn all characters
    [PunRPC]
    public void InGame()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayers();
        }
    }

    //Spawn characters that players selected
    public void SpawnPlayers()
    {
        GameObject playerobj;
        if (PhotonNetwork.IsMasterClient)
        {
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p1, spawnPositions[0].position, Quaternion.identity);
        }
        else
        {
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p2, spawnPositions[0].position, Quaternion.identity);
        }
        PlayerControl playscript = playerobj.GetComponent<PlayerControl>();
        playscript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    //Used in win or lose scene to go back to main menu
    public void GoBackToMenu()
    {
        Destroy(NetworkManager.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadScene("MenuScene");
    }
}
