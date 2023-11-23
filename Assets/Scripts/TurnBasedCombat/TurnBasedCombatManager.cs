/* Function: Manage the player and boss turns
   Author: Daniel Degollado Rodr�guez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using KaimiraGames;

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
    TurnBasedCombatPlayerDeath tbcPD;
    PersistenceManager pm;

    public TurnBasedCombatActions tbcPA;

    int randomIndex;
    System.Random random;

    public GameObject canvas = null;

    public float playerAttackMultiplier;
    public float playerDefenseMultiplier;
    public float BossAttackMultiplier;
    public float BossDefenseMultiplier;

    public bool skipP1Turn;
    public bool skipP2Turn;
    public bool skipBossTurn;

    public WeightedList<int> weightedPlayers;
    public int p1Health, p2Health;
    private PersistenceManager pm;

    public static TurnBasedCombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update.
    void Start()
    {
        pm = PersistenceManager.Instance;
        tbcTH = TurnBasedCombatTargetHandler.Instance;

        playersGameObject = GameObject.FindGameObjectsWithTag("Player").ToList();
        SpawnPlayer();

        photonView.RPC("InitializePlayers", RpcTarget.All);
        canvas = tbcPA.SetCorrespondingActionsMenu(players);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartTurn", RpcTarget.All);
        }

        pm = PersistenceManager.Instance;
        playerAttackMultiplier = 1;
        playerDefenseMultiplier = 1;
        BossAttackMultiplier = 1;
        BossDefenseMultiplier = 1;
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
                if(pm.CurrentHealth > 0)
                {
                    if (canvas != null)
                    {
                        canvas.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Canvas is null");
                    }
                }
                else
                {
                    EndTurn();
                }
            }
        }
        else
        {

            if (skipBossTurn)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    skipBossTurn = false;
                }
                else
                {
                    photonView.RPC("ReturnBossTurn", RpcTarget.All);
              
                }
            }
            else
            {
                Debug.Log("Boss turn");
                BossTurn();
                
            }
            EndTurn();
        }  
    }

    [PunRPC]
    void ReturnBossTurn()
    {
        skipBossTurn = false;
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
        if (PhotonNetwork.OfflineMode)
        {
            randomIndex = 0;
            tbcTH.TakeDamageTBC(randomIndex);
        }
        else
        {
            // actualiza los valores de las vidas actuales de los jugadores
            if (PhotonNetwork.IsMasterClient)
            {
                p1Health = pm.CurrentHealth;
            }
            else
            {
                p2Health = pm.CurrentHealth;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                /*random = new System.Random();
                randomIndex = random.Next(players.Count);*/
                // maybe code
                // crea una lista de los items con su indice y de peso pone la vida de los jugadores
                List<WeightedListItem<int>> playerWeights = new();
                {
                    new WeightedListItem<int>(0,p2Health);
                    new WeightedListItem<int>(1,p1Health);
                };

                // crea la lista con pesos y genera el valor del randomIndex
                weightedPlayers = new WeightedList<int>(playerWeights);
                randomIndex = weightedPlayers.Next();

                // end maybe code
                photonView.RPC("SyncronizeRandomIndex", RpcTarget.All, randomIndex);
                tbcTH.photonView.RPC("TakeDamageTBC", RpcTarget.All, randomIndex);
            }
        }

        /*if (players[randomIndex].IsLocal)
        {
            GameObject playerGameObject = players[randomIndex].tagObject as GameObject;
            if (playerGameObject.GetComponent<TurnBasedCombatPlayerDeath>().isDead == false)
            {
                tbcTH.photonView.RPC("TakeDamageTBC", RpcTarget.All, randomIndex);
            }
            else
            {
                Debug.Log("Dead");
                BossTurn();
            }
        }*/

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
