/* Function: control any variable that can be used by other scripts
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks, IDataPersistence
{
    // Singleton
    public static GameController instance = null;
    public static GameController Instance { get { return instance; } }
    //Have the players/characters spawned?
    public bool hasPlayersSpawned = false;
    public bool isPaused = false;
    public static int AlivePlayers = 0;
    public List<Vector3> savedSpawnPositions = new();

    void Start()
    {
        //AlivePlayers = PhotonNetwork.PlayerList.Length;
        if (!PhotonNetwork.OfflineMode)
        {
            AlivePlayers = 2;
        }
        else
        {
            AlivePlayers = 1;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //Save and load system
    public void LoadData(GameData data)
    {
        this.savedSpawnPositions = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {

    }
}
