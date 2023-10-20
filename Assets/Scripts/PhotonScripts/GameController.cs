/* Function: control any variable that can be used by other scripts
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{
    // Singleton
    public static GameController instance = null;
    public static GameController Instance { get { return instance; } }
    //Have the players/characters spawned?
    public bool hasPlayersSpawned = false;
    public static int AlivePlayers = 0;

    void Start()
    {
        // Assuming all players are alive at the start
        AlivePlayers = PhotonNetwork.PlayerList.Length;
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
}
