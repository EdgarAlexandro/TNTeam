/* Function: control the behaviour of the main menu
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuUIController : MonoBehaviourPunCallbacks
{
    //Canvas sections
    public GameObject createRoomWindow = null;
    public GameObject joinRoomWindow = null;
    public GameObject lobbyWindow = null;

    [Header("Create Room Menu")]
    public Button createRoomBtn = null;

    [Header("Join Room Menu")]
    public Button joinRoomBtn = null;

    [Header("Lobby")]
    public Button startGameBtn = null;
    public TextMeshProUGUI playerTextList = null;
    public Button closeRoomBtn = null;
    public Button leaveRoomBtn = null;
    public TextMeshProUGUI lobbyTitle = null;

    [Header("Character Selection")]
    public GameObject characterSelectWindow = null;
    // Strings of characters prefabs for Photon.Instantiate
    public string p1 = "";
    public string p2 = "";

    #region SingletonPattern
    public static MenuUIController instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
        }

    }
    #endregion

    //The player is connected to the Photon app
    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
        closeRoomBtn.gameObject.SetActive(false);
        leaveRoomBtn.gameObject.SetActive(false);
    }

    //The client has clicked the button to join a room
    public void JoinRoom(TMP_InputField _roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            NetworkManager.instance.JoinRoom(_roomName.text);
            photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
        }
    }

    //The host has clicked the button to create a room
    public void CreateRoom(TMP_InputField _roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            lobbyWindow.SetActive(true);
            createRoomWindow.SetActive(false);
            NetworkManager.instance.CreateRoom(_roomName.text);
            photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
        }
    }

    //The player (host or client) has joined a room
    public override void OnJoinedRoom()
    {
        //Only in multiplayer mode
        if (!PhotonNetwork.OfflineMode)
        {
            photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
            joinRoomWindow.SetActive(false);
            lobbyWindow.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
            {
                closeRoomBtn.gameObject.SetActive(true);
            }
            else
            {
                leaveRoomBtn.gameObject.SetActive(true);
                startGameBtn.gameObject.SetActive(false);
            }
        }
    }

    //Get the player's name from input
    public void GetPlayerName(TMP_InputField _playerName)
    {
        PhotonNetwork.NickName = _playerName.text;
    }

    // Remote Procedure Call to update players list, room name and buttons
    [PunRPC]
    public void UpdatePlayerInfo()
    {
        lobbyTitle.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        playerTextList.text = "List of players:" + "\n\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerTextList.text += player.NickName + "\n";
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.interactable = true;
        }
        else
        {
            startGameBtn.interactable = false;
        }
    }

    //Allows the host to close the room using OnMasterClientSwitched
    public void CloseLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyWindow.SetActive(false);
        createRoomWindow.SetActive(true);
    }

    //Allows the client to leave the room
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyWindow.SetActive(false);
        joinRoomWindow.SetActive(true);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    //Called when the master client is switched and is in lobby (to also be eliminated from room)
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient && lobbyWindow.activeSelf)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CloseConnection(player);
            }
            lobbyWindow.SetActive(false);
            joinRoomWindow.SetActive(true);
        }
    }

    //Updates the info in lobby when player leaves
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    // Remote Procedure Call to show the character selection window to all players
    [PunRPC]
    public void startCharacterSelection()
    {
        characterSelectWindow.SetActive(true);
        lobbyWindow.SetActive(false);
    }

    // Remote Procedure Call hide the character selection window to all players
    [PunRPC]
    public void endCharacterSelection()
    {
        characterSelectWindow.SetActive(false);
    }

    //Used by character selection window buttons to call the remote procedures
    public void characterSelectionWindowToggle(bool activeStatus)
    {
        if (activeStatus)
        {
            photonView.RPC("startCharacterSelection", RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("endCharacterSelection", RpcTarget.AllBuffered);
        }

    }

    //Used by character images in character selection window to save the strings of character prefabs
    public void characterSelect(string character)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            p1 = character;
        }
        else
        {
            p2 = character;
        }
    }

    //Changes the scene to "Main 1" to all connected players in room
    public void StartGame()
    {
        NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "Main 1");
    }
}