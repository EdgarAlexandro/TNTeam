using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviourPunCallbacks
{

    public GameObject createRoomWindow;
    public GameObject joinRoomWindow;
    public GameObject lobbyWindow;

    [Header("Create Room Menu")]
    public Button createRoomBtn;

    [Header("Join Room Menu")]
    public Button joinRoomBtn;

    [Header("Lobby")]
    public Button startGameBtn;
    public TextMeshProUGUI playerTextList;
    public Button closeRoomBtn;
    public Button leaveRoomBtn;
    public TextMeshProUGUI lobbyTitle;

    [Header("Character Selection")]
    public GameObject characterSelectWindow;
    public string p1;
    public string p2;

    #region SingletonPattern
    public static MenuUIController instance;

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

    

    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
        closeRoomBtn.gameObject.SetActive(false);
        leaveRoomBtn.gameObject.SetActive(false);
    }

    public void JoinRoom(TMP_InputField _roomName)
    {
        NetworkManager.instance.JoinRoom(_roomName.text);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    public void CreateRoom(TMP_InputField _roomName)
    {
        lobbyWindow.SetActive(true);
        createRoomWindow.SetActive(false);
        NetworkManager.instance.CreateRoom(_roomName.text);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    public override void OnJoinedRoom()
    {
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

    public void GetPlayerName(TMP_InputField _playerName)
    {
        PhotonNetwork.NickName = _playerName.text;
    }

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

    public void CloseLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyWindow.SetActive(false);
        createRoomWindow.SetActive(true);
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyWindow.SetActive(false);
        joinRoomWindow.SetActive(true);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    [PunRPC]
    public void startCharacterSelection()
    {
        characterSelectWindow.SetActive(true);
        lobbyWindow.SetActive(false);
    }

    [PunRPC]
    public void endCharacterSelection()
    {
        characterSelectWindow.SetActive(false);
    }

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


    public void StartGame()
    {
        if (PhotonNetwork.OfflineMode)
        {
            SceneManager.LoadScene("Main 1");
        }
        else
        {
            NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "Main 1");
        }
    }
}