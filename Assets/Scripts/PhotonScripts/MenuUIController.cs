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
using UnityEngine.Networking;

public class MenuUIController : MonoBehaviourPunCallbacks, IDataPersistence
{
    [Header("Canvas Windows")]
    public GameObject soloWindow = null;
    public GameObject coOpWindow = null;
    public GameObject webGLCodeWindow = null;
    public GameObject createRoomWindow = null;
    public GameObject joinRoomWindow = null;
    public GameObject lobbyWindow = null;

    [Header("Main Window")]
    public GameObject exitGameButton = null;

    [Header("Save and Load System")]
    public Button continueSoloGame = null;
    public Button continueCoOpGame = null;

    [Header("WEBGL Save System")]
    public TMP_InputField saveGameCode = null;
    public TMP_InputField saveFilePassword = null;
    public Button acceptCodeButton = null;
    public GameObject alertWindow = null;
    public GameObject loadingAlert = null;
    private bool callingAPI = false;
    private readonly string API_URL = "https://apitnteam.edgar2208.repl.co";

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
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            exitGameButton.SetActive(false);
        }

        if (!PhotonNetwork.OfflineMode && (characterSelectWindow.activeSelf || lobbyWindow.activeSelf))
        {
            if (lobbyWindow.activeSelf && PhotonNetwork.CurrentRoom.PlayerCount < 2)
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
    //In WEBGL this is ignored
    //Function used by "Solo" and "Co-Op" buttons
    public void ContinueGameButtonsVerification()
    {
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (!DataPersistenceManager.instance.CheckFile() && (Application.platform == RuntimePlatform.WebGLPlayer))
        //if (!DataPersistenceManager.instance.CheckFile() && !(Application.platform == RuntimePlatform.WebGLPlayer))
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

    //Used by canvas buttons on Windows or MacOS or by API response on webgl
    public void LoadGameWithData()
    {
        if (PhotonNetwork.OfflineMode)
        {
            DataPersistenceManager.instance.LoadGame();
            //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            //if (Application.platform != RuntimePlatform.WebGLPlayer) StartGame();
            if (Application.platform == RuntimePlatform.WebGLPlayer) StartGame();
        }
        else
        {
            coOpWindow.SetActive(false);
            createRoomWindow.SetActive(true);
            DataPersistenceManager.instance.LoadGame();
        }
    }

    //Checks player is playing in WEBGL (using a different save system)
    public void ContinueGameButtonsActions()
    {
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            webGLCodeWindow.SetActive(true);
            soloWindow.SetActive(false);
            coOpWindow.SetActive(false);
        }
        else
        {
            LoadGameWithData();
        }
    }

    //Starts an "Animation" in loading text
    private IEnumerator LoadingAnimation()
    {
        int dots = 0;
        loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text = "Loading";
        loadingAlert.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while (callingAPI)
        {
            if(dots < 3)
            {
                loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text += ".";
                dots++;
            }
            else
            {
                loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text = "Loading";
                dots = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    //Method used when playing in webGL to check if file exits in API
    //Used by accept button in WEBGL panel
    public void LoadGameWEBGL()
    {
        acceptCodeButton.interactable = false;
        callingAPI = true;
        string code = saveGameCode.text;
        string password = saveFilePassword.text;
        StartCoroutine(APIFileVerification(code + ".json", password));
    }

    //Used by "Accept" button in load game (WEBGL) window to call the API
    //The request checks if a file with the code provided exists and if its password is the same as the one provided
    IEnumerator APIFileVerification(string fileName, string password)
    {
        bool saveFileExists = false;
        bool apiCallWorks = false;
        string url;
        if (PhotonNetwork.OfflineMode)
        {
            url = API_URL + $"/read_file_data?fileName={fileName}&mode=Solo&password={password}";
        }
        else
        {
            url = API_URL + $"/read_file_data?fileName={fileName}&mode=Multiplayer&password={password}";
        }
        StartCoroutine(LoadingAnimation());
        while (!apiCallWorks)
        {
            using UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error info: {www.error}");
                Debug.Log("La API probablemente esta apagada... starting API");
                yield return new WaitForSeconds(3);
            }
            else
            {
                Debug.Log($"Respuesta: {www.downloadHandler.text}");

                if (www.downloadHandler.text == "true")
                {
                    saveFileExists = true;
                }
                apiCallWorks = true;
            }
        }
        StopCoroutine(LoadingAnimation());
        loadingAlert.SetActive(false);
        if (saveFileExists)
        {
            saveGameCode.text = "";
            saveFilePassword.text = "";
            GameController.instance.fileSaveName = fileName;
            GameController.instance.password = password;
            LoadGameWithData();
            acceptCodeButton.interactable = true;
        }
        else
        {
            alertWindow.SetActive(true);
            yield return new WaitForSeconds(4);
            alertWindow.SetActive(false);
            acceptCodeButton.interactable = true;
        }
        callingAPI = false;
}

    //Method used when playing in webGL to exit its load system window
    //Used by exit button in WEBGL panel
    public void ExitLoadGameWEBGL()
    {
        webGLCodeWindow.SetActive(false);
        if (PhotonNetwork.OfflineMode) soloWindow.SetActive(true);
        else coOpWindow.SetActive(true);
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
            if (!player.IsMasterClient) PhotonNetwork.CloseConnection(player);
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
        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode) CloseLobby();
        if (PhotonNetwork.OfflineMode) soloWindow.SetActive(true);
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