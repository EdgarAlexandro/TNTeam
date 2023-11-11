/* Function: manages the methods to save, load and create new games (Keeps track of current gameData)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 02/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class DataPersistenceManager : MonoBehaviourPunCallbacks
{
    [Header("File Storage Config")]
    [SerializeField] private string soloFileName;
    [SerializeField] private string multiplayerFileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public bool isNewGame = false;

    public static DataPersistenceManager instance { get; private set; }

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

    //Starts the save and load system
    //Used by menu buttons: "Solo" and "Co-Op"
    public void StartSaveSystem(bool solo)
    {
        if (solo) this.dataHandler = new FileDataHandler(Application.persistentDataPath, soloFileName, useEncryption);
        else this.dataHandler = new FileDataHandler(Application.persistentDataPath, multiplayerFileName, useEncryption);
    }

    //Checks if file already exists
    public bool CheckFile()
    {
        return dataHandler.FileValidation();
    }

    //Finds all scripts that implement the save/load system
    private void Update()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceobjects();
    }

    //Starts a new game data class with default values
    public void NewGame()
    {
        this.isNewGame = true;
        this.gameData = new GameData();
        //push the Loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    //Used by continue buttons to load saved data
    public void LoadGame()
    {
        this.isNewGame = false;
        //Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();
        //push the Loaded data to all other scripts that need it
        foreach ( IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("Loaded data");
    }

    //Called by photon when player joins room, sends data to load
    //Multiplayer games data is stored on MasterClient side
    public void PlayerJoined()
    {
        if (isNewGame)
        {
            photonView.RPC("LoadClientData", RpcTarget.Others, "");
        }
        else
        {
            LoadGame();
        }
    }

    //Loads the data to client when they join the room
    [PunRPC]
    public void LoadClientData(string dataString)
    {
        if(dataString != "")
        {
            this.gameData = JsonUtility.FromJson<GameData>(dataString);
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }
        else
        {
            NewGame();
        }
        
    }

    //Calls all the SaveData functions in all scripts that implement it
    public void SaveGame()
    {
        //pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //save that data to a file using the data handler
        dataHandler.Save(gameData);
        Debug.Log("Saved data");
    }

    /*
    //TODO the game should be saved on quit?
    private void OnApplicationQuit()
    {
        SaveGame();
    }*/

    //Finds all scripts that implement the save/load system
    private List<IDataPersistence> FindAllDataPersistenceobjects()
    {
        HashSet<IDataPersistence> uniqueDataPersistenceObjects = new HashSet<IDataPersistence>();

        foreach (var obj in FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>())
        {
            uniqueDataPersistenceObjects.Add(obj);
        }

        foreach (var obj in FindObjectsOfType<MonoBehaviourPunCallbacks>().OfType<IDataPersistence>())
        {
            uniqueDataPersistenceObjects.Add(obj);
        }

        return new List<IDataPersistence>(uniqueDataPersistenceObjects);
    }
}
