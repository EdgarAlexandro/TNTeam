using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnController : MonoBehaviourPunCallbacks
{
    //Instancia (singleton)
    public static SpawnController instance;

    public Transform[] spawnPositions;
    public PlayerControl[] players;

    [SerializeField] private int playerInGame;  

    private void Awake()
    {
        instance = this;
        if (!GameController.instance.hasPlayersSpawned)
        {
            if (PhotonNetwork.OfflineMode)
            {
                GameObject playerobj;
                playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p1, spawnPositions[0].position, Quaternion.identity);

            }
            else
            {
                players = new PlayerControl[PhotonNetwork.PlayerList.Length];
                photonView.RPC("InGame", RpcTarget.AllBuffered);
            }
            GameController.instance.hasPlayersSpawned = true;
        }
        
    }

    [PunRPC]
    void InGame()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayers();
        }
    }

    void SpawnPlayers()
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
        

        //forma larga
        //playerobj.GetComponent<PlayerControl>().photonview.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);

        //uso de variable para facil lectura
        PlayerControl playscript = playerobj.GetComponent<PlayerControl>();
        playscript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);


    }

    void GoBackToMenu()
    {
        Destroy(NetworkManager.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadScene("MenuScene");
    }
}
