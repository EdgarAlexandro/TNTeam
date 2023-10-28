/* Function: control the behaviour of photon unity network
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region SingletonPattern
    public static NetworkManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    #endregion

    #region ConnectionToServer
    //Multiplayer button in main menu
    public void MultiplayerMode()
    {
        StartCoroutine(Reconnect(false));
    }

    //Solo button in main menu
    public void SoloMode()
    {
        StartCoroutine(Reconnect(true));
    }

    //Allows the player to switch between solo and multiplayer using photon offline mode
    public IEnumerator Reconnect(bool modeOffline)
    {
        PhotonNetwork.Disconnect();
        yield return new WaitForSeconds(0.05f);
        if (!modeOffline) PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(0.05f);
        PhotonNetwork.OfflineMode = modeOffline;
        yield return new WaitForSeconds(0.05f);
        if (modeOffline) CreateRoom("SoloPlayerRoom");
    }
    #endregion

    #region RoomLogic
    public void CreateRoom(string _name)
    {
        // Max players per room set to 2
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 2,
        };
        PhotonNetwork.CreateRoom(_name, roomOptions);
    }

    //DELETE (purely informational)
    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        Debug.Log(string.Format("Created Room: {0}", PhotonNetwork.CurrentRoom.Name));
    }
    #endregion

    #region ConnectionToRoom
    public void JoinRoom(string _name)
    {
        PhotonNetwork.JoinRoom(_name);
    }

    // Remote Procedure Call to change the scene for all players connected in room
    [PunRPC]
    public void LoadScene(string _nameScene)
    {
        PhotonNetwork.LoadLevel(_nameScene);
    }
    #endregion
}