using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region SingletonPattern
    public static NetworkManager instance;

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
    public void MultiplayerMode()
    {
        StartCoroutine(Reconnect(false));
    }

    public void SoloMode()
    {
        StartCoroutine(Reconnect(true));
    }
    
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

    [PunRPC]
    public void LoadScene(string _nameScene)
    {
        PhotonNetwork.LoadLevel(_nameScene);
    }
    #endregion
}