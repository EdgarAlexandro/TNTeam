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

    [Header("Lista de items")]
    public List<ItemData> items;

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

    private void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (!ignoredScenes.Contains(currentScene))
        {
            CurrentScene = currentScene;
        }
    }

    //Loads data to persistence manager
    public void LoadData(GameData data)
    {
        this.CurrentScene = data.scene;
        if (PhotonNetwork.IsMasterClient) photonView.RPC("LoadHealthAndMagic", RpcTarget.All, data.playersHealth.ToArray(), data.playersMagic.ToArray());
        this.CurrentKeys = data.obtainedKeys;
        this.LeverCounter = data.obtainedLevers;

        List<string> playerInv;
        if (PhotonNetwork.IsMasterClient) playerInv = data.playerOneInventory;
        else playerInv = data.playerTwoInventory;
        foreach (string itemName in playerInv)
        {
            foreach (ItemData item in items)
            {
                if (item.name == itemName)
                {
                    if (data.charactersSelected[0] == "Reina Corazones" || data.charactersSelected[1] == "Reina Corazones")
                    {
                        this.ReinaCorazones.AddItem(item);
                    }
                    else if (data.charactersSelected[0] == "Rey Picas" || data.charactersSelected[1] == "Rey Picas")
                    {
                        this.ReyPicas.AddItem(item);
                    }
                    else if (data.charactersSelected[0] == "Reina Treboles" || data.charactersSelected[1] == "Reina Treboles")
                    {
                        this.ReinaTreboles.AddItem(item);
                    }
                    else if (data.charactersSelected[0] == "Rey Diamantes" || data.charactersSelected[1] == "Rey Diamantes")
                    {
                        this.ReyDiamantes.AddItem(item);
                    }
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
            Debug.Log("Master has " + typeToSave);
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.IsMasterClient && player.CustomProperties != null && player.CustomProperties.ContainsKey(typeToSave))
            {
                Debug.Log("Client has " + typeToSave);
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
