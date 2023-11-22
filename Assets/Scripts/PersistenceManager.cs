using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PersistenceManager : MonoBehaviourPunCallbacks, IDataPersistence
{
    private static PersistenceManager instance;
    public static PersistenceManager Instance { get { return instance; } }

    private List<string> ignoredScenes = new List<string> { "CutsceneIntro", "LoseScene", "StartMenu", "WinScene" };
    public string CurrentScene;
    public int CurrentHealth;
    public int MaxHealth;
    public int MaxMagic;
    public int CurrentMagic;
    public int MaxKeys;
    public int CurrentKeys;
    public int LeverCounter;

    [Header("Inventarios")]
    public Inventory ReinaCorazones;
    public Inventory ReyPicas;
    public Inventory ReinaTreboles;
    public Inventory ReyDiamantes;
    public Inventory cardInventory;

    [Header("Lista de items")]
    public List<ItemData> items;
    public List<CardData> cards;

    [Header("WebGL data")]
    public List<int> healths;
    public List<int> magics;

    // Singleton
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

    private void FixedUpdate()
    {
        //This updates the current scene name to be ready when the saveData function is called
        string currentScene = SceneManager.GetActiveScene().name;
        if (!ignoredScenes.Contains(currentScene))
        {
            this.CurrentScene = currentScene;
        }
    }

    //Clears all inventory objects on quit
    public void CleanAllInventories()
    {
        ReinaCorazones.items.Clear();
        ReyPicas.items.Clear();
        ReinaTreboles.items.Clear();
        ReyDiamantes.items.Clear();
        cardInventory.cards.Clear();
    }

    private void OnApplicationQuit()
    {
        CleanAllInventories();
    }

    //Loads data to persistence manager
    public void LoadData(GameData data)
    {
        //------------------------------------------------Scene------------------------------------------------
        this.CurrentScene = data.scene;

        //------------------------------------------------Health and Magic------------------------------------------------

        //Data is loaded immediately because there is a master client
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        //if (PhotonNetwork.IsMasterClient && Application.platform != RuntimePlatform.WebGLPlayer) photonView.RPC("LoadHealthAndMagic", RpcTarget.All, data.playersHealth.ToArray(), data.playersMagic.ToArray());
        if (Application.platform != RuntimePlatform.WebGLPlayer && !DataPersistenceManager.instance.isNewGame && PhotonNetwork.InRoom) photonView.RPC("LoadHealthAndMagic", RpcTarget.All, data.playersHealth.ToArray(), data.playersMagic.ToArray());
        //if (PhotonNetwork.IsMasterClient && Application.platform == RuntimePlatform.WebGLPlayer) photonView.RPC("LoadHealthAndMagic", RpcTarget.All, data.playersHealth.ToArray(), data.playersMagic.ToArray());

        //Data is stored in lists to use later (ONLY WEBGL)
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (PhotonNetwork.OfflineMode)
            {
                this.CurrentHealth = data.playersHealth[0];
                this.CurrentMagic = data.playersMagic[0];
            }
            else
            {
                this.healths = data.playersHealth;
                this.magics = data.playersMagic;
                if (PhotonNetwork.IsMasterClient)
                {
                    this.CurrentHealth = healths[0];
                    this.CurrentMagic = magics[0];
                }
                else
                {
                    this.CurrentHealth = healths[1];
                    this.CurrentMagic = magics[1];
                }
            }
        }

        //------------------------------------------------Keys and Levers------------------------------------------------
        this.CurrentKeys = data.obtainedKeys;
        this.LeverCounter = data.obtainedLevers;

        //------------------------------------------------Inventory------------------------------------------------
        CleanAllInventories();
        List<string> playerInv;
        int characterSelectedStorage;
        if (PhotonNetwork.IsMasterClient)
        {
            playerInv = data.playerOneInventory;
            characterSelectedStorage = 0;
        }
        else
        {
            playerInv = data.playerTwoInventory;
            characterSelectedStorage = 1;
        }
        foreach (string itemName in playerInv)
        {
            foreach (ItemData item in items)
            {
                if (item.name == itemName)
                {
                    if (data.charactersSelected[characterSelectedStorage] == "Reina Corazones")
                    {
                        this.ReinaCorazones.AddItem(item);
                    }
                    else if (data.charactersSelected[characterSelectedStorage] == "Rey Picas")
                    {
                        this.ReyPicas.AddItem(item);
                    }
                    else if (data.charactersSelected[characterSelectedStorage] == "Reina Treboles")
                    {
                        this.ReinaTreboles.AddItem(item);
                    }
                    else if (data.charactersSelected[characterSelectedStorage] == "Rey Diamantes")
                    {
                        this.ReyDiamantes.AddItem(item);
                    }
                }
            }
        }

        //------------------------------------------------Cards Inventory------------------------------------------------
        List<string> playerCardInv;
        if (PhotonNetwork.IsMasterClient) playerCardInv = data.playerOneCardInventory;
        else playerCardInv = data.playerTwoCardInventory;
        foreach (string cardName in playerCardInv)
        {
            foreach (CardData card in cards)
            {
                if (card.name == cardName)
                {
                    this.cardInventory.AddCard(card);
                }
            }
        }
    }

    //Saves data from persistence manager to json
    public void SaveData(ref GameData data)
    {
        data.scene = this.CurrentScene;
        data.playersHealth = SavePlayersHealthOrMagic("Health");
        data.playersMagic = SavePlayersHealthOrMagic("Magic");
        data.charactersSelected = SaveCharactersSelected();
        data.obtainedKeys = this.CurrentKeys;
        data.obtainedLevers = this.LeverCounter;
    }

    //saves individual data using photon custom properties
    public List<int> SavePlayersHealthOrMagic(string typeToSave)
    {
        List<int> listOfAttributes = new List<int>();

        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey(typeToSave))
        {
            listOfAttributes.Add((int)PhotonNetwork.MasterClient.CustomProperties[typeToSave]);
            //Debug.Log("Master has " + typeToSave);
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.IsMasterClient && player.CustomProperties != null && player.CustomProperties.ContainsKey(typeToSave))
            {
                //Debug.Log("Client has " + typeToSave);
                listOfAttributes.Add((int)player.CustomProperties[typeToSave]);
            }
        }
        return listOfAttributes;
    }

    //Rpc to load individual data to all players
    [PunRPC]
    public void LoadHealthAndMagic(int[] healths, int[] magics)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            this.CurrentHealth = healths[0];
            this.CurrentMagic = magics[0];
        }
        else
        {
            this.CurrentHealth = healths[1];
            this.CurrentMagic = magics[1];
        }
    }

    //ONLY IN WEBGL Storage for magic and health values
    //This function is runned by both players
    //The health and magic values were stored in lists when the api call was made and loaded until the creation of the room
    //because there was no master client when api call was made
    public override void OnJoinedRoom()
    {
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                this.CurrentHealth = healths[0];
                this.CurrentMagic = magics[0];
            }
            else
            {
                this.CurrentHealth = healths[1];
                this.CurrentMagic = magics[1];
            }
        }
    }

    //Saves the selected characters in game by using the gameobjects name
    public List<string> SaveCharactersSelected()
    {
        List<string> listOfCharacters = new();
        string aux = "";
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                listOfCharacters.Add(player.name.Replace("(Clone)", ""));
            }
            else
            {
                aux = player.name.Replace("(Clone)", "");
            }
        }
        listOfCharacters.Add(aux);
        return listOfCharacters;
    }
}
