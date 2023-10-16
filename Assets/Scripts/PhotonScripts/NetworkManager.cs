/* Function: control the behaviour of photon unity network
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    IEnumerator Reconnect(bool modeOffline)
    {
        PhotonNetwork.Disconnect();
        yield return new WaitForSeconds(0.05f);
        if (!modeOffline) PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(0.05f);
        PhotonNetwork.OfflineMode = modeOffline;
        yield return new WaitForSeconds(0.05f);
        if (modeOffline) CreateRoom("testRoom");
    }
    #endregion

    #region RoomLogic
    public void CreateRoom(string _name)
    {
        PhotonNetwork.CreateRoom(_name);
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