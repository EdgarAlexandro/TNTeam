/* Function: control the behaviour of the players spawn
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class SpawnController : MonoBehaviourPunCallbacks, IDataPersistence
{
    //Instancia (singleton)
    public static SpawnController instance = null;

    //public Transform[] spawnPositions = null;
    public List<Vector3> spawnPositions = new List<Vector3>();
    //public PlayerControl[] players = null;

    [SerializeField] private int playerInGame = 0;

    public GameObject canvasPrefab = null;

    public void LoadData(GameData data)
    {

    }

    //Saves the players position
    public void SaveData(ref GameData data)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<Vector3> playersLastPositions = new List<Vector3>();
        foreach (GameObject player in players)
        {
            playersLastPositions.Add(player.transform.position);
        }
        data.playerPosition = playersLastPositions;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        spawnPositions = GameController.instance.savedSpawnPositions;
        if(spawnPositions.Count == 0)
        {
            // Find all objects with the tag "SpawnPoint" and add their transforms to the list
            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
            foreach (GameObject spawnPointObject in spawnPointObjects)
            {
                Vector3 spawnPointTransform = spawnPointObject.transform.position;
                spawnPositions.Add(spawnPointTransform);
            }
        }

        if (!GameController.instance.hasPlayersSpawned)
        {
            //If player is in offline mode, just spawns 1 character
            if (PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.Instantiate(MenuUIController.instance.p1, GetNextSpawnPosition(), Quaternion.identity);
                GameObject canvasp1 = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                canvasp1.name = "Canvas Player 1";
            }
            else
            {
                //players = new PlayerControl[PhotonNetwork.PlayerList.Length];
                photonView.RPC("InGame", RpcTarget.AllBuffered);
            }
            GameController.instance.hasPlayersSpawned = true;
        }
    }

    private Vector3 GetNextSpawnPosition()
    {
        if (spawnPositions.Count > 1 && !PhotonNetwork.OfflineMode || spawnPositions.Count > 0 && PhotonNetwork.OfflineMode)
        {
            Vector3 spawnPosition;
            if (PhotonNetwork.IsMasterClient)
            {
                spawnPosition = spawnPositions[0];
                
            }
            else
            {
                spawnPosition = spawnPositions[1];
            }
            return spawnPosition;
        }
        else
        {
            Debug.LogError("No spawn positions available!");
            return Vector3.zero;
        }
    }

    //Remote procedure call to spawn all characters
    [PunRPC]
    public void InGame()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayers();
        }
    }

    //Spawn characters that players selected
    public void SpawnPlayers()
    {
        GameObject playerobj;
        if (PhotonNetwork.IsMasterClient)
        {
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p1, GetNextSpawnPosition(), Quaternion.identity);
            GameObject canvasp1 = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            canvasp1.name = "Canvas Player 1";
        }
        else
        {
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p2, GetNextSpawnPosition(), Quaternion.identity);
            GameObject canvasp2 = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            canvasp2.name = "Canvas Player 2";
        }
        PlayerControl playscript = playerobj.GetComponent<PlayerControl>();
        playscript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    //Used in win or lose scene to go back to main menu
    public void GoBackToMenu()
    {
        Destroy(NetworkManager.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadScene("MenuScene");
    }
}
