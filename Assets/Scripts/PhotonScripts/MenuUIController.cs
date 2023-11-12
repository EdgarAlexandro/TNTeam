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

public class MenuUIController : MonoBehaviourPunCallbacks, IDataPersistence
{
    [Header("Canvas Windows")]
    public GameObject soloWindow = null;
    public GameObject coOpWindow = null;
    public GameObject createRoomWindow = null;
    public GameObject joinRoomWindow = null;
    public GameObject lobbyWindow = null;

    [Header("Save and Load System")]
    public Button continueSoloGame = null;
    public Button continueCoOpGame = null;

    [Header("Create Room Menu")]
    public Button createRoomBtn = null;
    public TMP_InputField playerNameCreate = null;
    public TMP_InputField roomNameCreate = null;

    [Header("Join Room Menu")]
    public Button joinRoomBtn = null;
    public TMP_InputField playerNameJoin = null;
    public TMP_InputField roomNameJoin = null;

    [Header("Lobby")]
    public Button startGameBtn = null;
    public TextMeshProUGUI playerTextList = null;
    public Button closeRoomBtn = null;
    public Button leaveRoomBtn = null;
    public TextMeshProUGUI lobbyTitle = null;
    private bool roomWasClosed = false;
    private bool wasHost = false;

    [Header("Character Selection")]
    public GameObject characterSelectWindow = null;
    // Strings of characters prefabs for Photon.Instantiate
    public string p1 = "";
    public string p2 = "";
    public Button ReinaCorazones = null;
    public Button ReyDiamantes = null;
    public Button ReinaTreboles = null;
    public Button ReyPicas = null;
    public Button acceptCharacterSelection = null;

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
        createRoomBtn.interactable = false;
        joinRoomBtn.interactable = false;
        continueSoloGame.interactable = false;
        continueCoOpGame.interactable = false;
    }
    #endregion

    //loads the selected characters from json
    public void LoadData(GameData data)
    {
        this.p1 = data.charactersSelected[0];
        this.p2 = data.charactersSelected[1];
    }

    public void SaveData(ref GameData data)
    {
        
    }

    //Stops the host to start the game if one of the players hasnt picked a character or hasnt joined the room
    private void Update()
    {
        if (!PhotonNetwork.OfflineMode && (characterSelectWindow.activeSelf || lobbyWindow.activeSelf ))
        {
            if(lobbyWindow.activeSelf && PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                startGameBtn.interactable = false;
            }
            else
            {
                startGameBtn.interactable = true;
            }
            if (p1 == "" || p2 == "")
            {
                acceptCharacterSelection.interactable = false;
            }
            else
            {
                acceptCharacterSelection.interactable = true;
            }
        }
        else
        {
            if (p1 == "")
            {
                acceptCharacterSelection.interactable = false;
            }
            else
            {
                acceptCharacterSelection.interactable = true;
            }
        }
    }

    //Checks if data file exists, if it doesnt you need to start a new game
    //Function used by "Solo" and "Co-Op" buttons
    public void ContinueGameButtons()
    {
        if (!DataPersistenceManager.instance.CheckFile())
        {
            continueSoloGame.interactable = false;
            continueCoOpGame.interactable = false;
        }
        else
        {
            continueSoloGame.interactable = true;
            continueCoOpGame.interactable = true;
        }
    }

    //The player is connected to the Photon app
    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
        closeRoomBtn.gameObject.SetActive(false);
        leaveRoomBtn.gameObject.SetActive(false);
    }

    //The client has clicked the button to join a room
    //Gets the player name (nickname) and room name
    public void JoinRoom(TMP_InputField _roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            NetworkManager.instance.JoinRoom(_roomName.text);
        }
    }

    //The host has clicked the button to create a room
    //Gets the player name (nickname) and room name
    public void CreateRoom(TMP_InputField _roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            NetworkManager.instance.CreateRoom(_roomName.text);
        }
    }

    //The player host/client has created/joined a room
    public override void OnJoinedRoom()
    {
        //Only in multiplayer mode
        if (!PhotonNetwork.OfflineMode)
        {
            createRoomWindow.SetActive(false);
            joinRoomWindow.SetActive(false);
            PhotonNetwork.EnableCloseConnection = true;
            photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
            playerNameCreate.text = "";
            roomNameCreate.text = "";
            playerNameJoin.text = "";
            roomNameJoin.text = "";
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

    //Allows the host to close the room using CloseConnection and OnLeftRoom
    public void CloseLobby()
    {
        photonView.RPC("RoomClosed", RpcTarget.All);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(!player.IsMasterClient) PhotonNetwork.CloseConnection(player);
        }
        PhotonNetwork.LeaveRoom();        
    }

    // Remote Procedure Call to update roomWasClosed to true to all players and keeps track of who the host was
    [PunRPC]
    public void RoomClosed()
    {
        roomWasClosed = true;
        if (PhotonNetwork.IsMasterClient) wasHost = true;
    }

    //Allows the client to leave the room
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyWindow.SetActive(false);
        joinRoomWindow.SetActive(true);
    }

    // Only used when players left the room because it was closed
    // Takes players to a specific screen (depending on whether it was the host or client) and reconnects them to photon
    public override void OnLeftRoom()
    {
        if (roomWasClosed)
        {
            lobbyWindow.SetActive(false);
            if (wasHost)
            {
                createRoomWindow.SetActive(true);
            }
            else
            {
                joinRoomWindow.SetActive(true);
            }
            StartCoroutine(NetworkManager.instance.Reconnect(false));
            roomWasClosed = false;
            wasHost = false;
        }
    }

    //Updates the info in lobby when player leaves
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.InRoom) photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    //Updates the saved data for the client when they join the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient) DataPersistenceManager.instance.PlayerJoined();
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
        if (PhotonNetwork.IsMasterClient) CloseLobby();
    }

    //Used by character selection window buttons to call the remote procedures
    public void characterSelectionWindowToggle(bool activeStatus)
    {
        if (activeStatus)
        {
            if (DataPersistenceManager.instance.isNewGame)
            {
                photonView.RPC("startCharacterSelection", RpcTarget.AllBuffered);
            }
            else
            {
                StartGame();
            }
            
        }
        else
        {
            photonView.RPC("endCharacterSelection", RpcTarget.AllBuffered);
        }

    }

    //Activates all the buttons of character select window when a new character is picked by player
    private void ActivateButtons()
    {
        ReinaCorazones.interactable = true;
        ReyDiamantes.interactable = true;
        ReinaTreboles.interactable = true;
        ReyPicas.interactable = true;
    }

    //Changes the character strings (p1 and p2) in photon network and prevents the player to pick the same character as their friend
    [PunRPC]
    public void ChangeCharacterString(int player, string character)
    {
        if (player == 1) p1 = character;
        else p2 = character;

        string otherPlayer;
        if (PhotonNetwork.IsMasterClient)
        {
            otherPlayer = p2;
            
        }
        else
        {
            otherPlayer = p1;
        }
        ActivateButtons();
        switch (otherPlayer)
        {
            case "Reina Corazones":
                ReinaCorazones.interactable = false;
                break;
            case "Rey Diamantes":
                ReyDiamantes.interactable = false;
                break;
            case "Reina Treboles":
                ReinaTreboles.interactable = false;
                break;
            case "Rey Picas":
                ReyPicas.interactable = false;
                break;
        }
    }

    //Used by character images in character selection window to save the strings of character prefabs
    public void characterSelect(string character)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeCharacterString", RpcTarget.All, 1, character);
        }
        else
        {
            photonView.RPC("ChangeCharacterString", RpcTarget.All, 2, character);
        }
    }

    //Changes the scene to the saved scene to all connected players in room (start scene: Main 1)
    public void StartGame()
    {
        NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, PersistenceManager.Instance.CurrentScene);
    }
}