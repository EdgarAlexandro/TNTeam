/* Function: Manage the player and boss turns
   Author: Daniel Degollado Rodr�guez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class TurnBasedCombatManager : MonoBehaviourPunCallbacks
{
    public List<PlayerInNetwork> players;
    private int currentPlayerIndex = 0;
    public List<GameObject> playersGameObject = new List<GameObject>();
    public GameObject boss;
    public List<GameObject> prefabList = new List<GameObject>();
    public List<Transform> spawnPositions = new List<Transform>();
    public List<Transform> bossSpawnPositions = new List<Transform>();
    TurnBasedCombatTargetHandler tbcTH;
    int randomIndex;
    System.Random random;

    public GameObject canvas = null;

    public static TurnBasedCombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update.
    void Start()
    {
        tbcTH = TurnBasedCombatTargetHandler.Instance;
        playersGameObject = GameObject.FindGameObjectsWithTag("Player").ToList();
        SpawnPlayer();
        photonView.RPC("InitializePlayers", RpcTarget.All);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartTurn", RpcTarget.All);
        }
    }
    // Get the players in the photon network.
    [PunRPC]
    void InitializePlayers()
    {
        players = new List<PlayerInNetwork>();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            PlayerInNetwork newPlayer = new PlayerInNetwork(player);
            players.Add(newPlayer);
        }
    }
    // Starts turn for a player or boss. It activates the actions menu for the player that has the current turn.
    [PunRPC]
    void StartTurn()
    {
        if(currentPlayerIndex < players.Count)
        {
            PlayerInNetwork currentPlayer = players[currentPlayerIndex];
            //Debug.Log("It's " + currentPlayer.Name + "'s turn.");

            if (currentPlayer.IsLocal)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Boss turn");
            BossTurn();
            EndTurn();
        }  
    }
    // Starts the punRPC SelectTarget.
    void BossTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SelectTarget", RpcTarget.All);
        }
    }
    // Boss selects which player to attack.
    [PunRPC]
    public void SelectTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            random = new System.Random();
            randomIndex = random.Next(players.Count);
            photonView.RPC("SyncronizeRandomIndex", RpcTarget.All, randomIndex);
            tbcTH.photonView.RPC("TakeDamageTBC", RpcTarget.All, randomIndex);
        }
    }
    // PunRPC that makes all players have the same random index, so the same player is attacked. Takes the index as a parameter.
    [PunRPC]
    private void SyncronizeRandomIndex(int index)
    {
        randomIndex = index;
    }
    // End the current turn and start a new one
    public void EndTurn()
    {
        canvas.SetActive(false);
        // Move to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % (players.Count + 1); 
        Debug.Log(currentPlayerIndex);

        // Notify all clients of the new turn
        photonView.RPC("UpdateCurrentPlayer", RpcTarget.All, currentPlayerIndex);  
        // PunRPC to start a new turn
        photonView.RPC("StartTurn", RpcTarget.All);
    }
    // PunRPC to that makes all players have the same current player, so the same player has the turn for both clients.
    [PunRPC]
    void UpdateCurrentPlayer(int index)
    {
        currentPlayerIndex = index;
    }
    // Spawns the turn based combat character depending which one was selected by the player in the character selection screen.
    private void SpawnPlayer()
    {
        foreach (GameObject player in playersGameObject)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                GameObject characterPrefab = GetSelectedCharacterPrefab(player);
                PhotonNetwork.Instantiate(characterPrefab.name, GetNextSpawnPosition().position, Quaternion.identity);
            }
        }
    }
    // Get the turn based combat character prefab to instantiate. Takes the player's gameobject as a parameter.
    private GameObject GetSelectedCharacterPrefab(GameObject player)
    {
        string playerName = player.name.Replace("(Clone)", "");
        foreach (GameObject prefab in prefabList)
        {
            if (prefab.name.StartsWith(playerName))
            {
                return prefab;
            }
        }
        return null;
    }
    // Depending if player is master client or not, assign a spawning position for their character. If its offline it just asigns the spawning position.
    private Transform GetNextSpawnPosition()
    {
        if (spawnPositions.Count > 0)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Transform spawnPosition = spawnPositions[2];
                return spawnPosition;
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Transform spawnPosition = spawnPositions[1];
                    return spawnPosition;
                }
                else
                {
                    Transform spawnPosition = spawnPositions[0];
                    return spawnPosition;
                }
            }
        }
        else
        {
            Debug.LogError("No spawn positions available!");
            return null;
        }
    }
}