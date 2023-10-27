/* Function: control the behaviour of the players spawn
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnController : MonoBehaviourPunCallbacks
{
    //Instancia (singleton)
    public static SpawnController instance = null;

    //public Transform[] spawnPositions = null;
    public List<Transform> spawnPositions = new List<Transform>();
    public PlayerControl[] players = null;

    [SerializeField] private int playerInGame = 0;

    public GameObject canvasPrefab = null;

    private void Awake()
    {
        instance = this;
        if (!GameController.instance.hasPlayersSpawned)
        {
            //If player is in offline mode, just spawns 1 character
            if (PhotonNetwork.OfflineMode)
            {

                PhotonNetwork.Instantiate(MenuUIController.instance.p1, GetNextSpawnPosition().position, Quaternion.identity);

                GameObject canvasp1 = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                canvasp1.name = "Canvas Player 1";

            }
            else
            {
                players = new PlayerControl[PhotonNetwork.PlayerList.Length];
                photonView.RPC("InGame", RpcTarget.AllBuffered);
            }
            GameController.instance.hasPlayersSpawned = true;
        }
    }

    private void Start()
    {
        // Find all objects with the tag "SpawnPoint" and add their transforms to the list
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject spawnPointObject in spawnPointObjects)
        {
            Transform spawnPointTransform = spawnPointObject.transform;
            spawnPositions.Add(spawnPointTransform);
        }
    }

    private Transform GetNextSpawnPosition()
    {
        if (spawnPositions.Count > 0)
        {
            Transform spawnPosition = spawnPositions[playerInGame % spawnPositions.Count];
            //playerInGame++;
            return spawnPosition;
        }
        else
        {
            Debug.LogError("No spawn positions available!");
            return null;
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
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p1, GetNextSpawnPosition().position, Quaternion.identity);
            GameObject canvasp1 = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            canvasp1.name = "Canvas Player 1";
        }
        else
        {
            playerobj = PhotonNetwork.Instantiate(MenuUIController.instance.p2, GetNextSpawnPosition().position, Quaternion.identity);
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
