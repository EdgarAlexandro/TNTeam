/* Function: manages the methods to save, load and create new games (Keeps track of current gameData)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 02/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.Networking;
using System.Text;
using System.Net;

public class DataPersistenceManager : MonoBehaviourPunCallbacks
{
    [Header("File Storage Config")]
    private string API_URL = "https://apitnteam.edgar2208.repl.co";
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

        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(GetAPIFileData(GameController.instance.fileSaveName));
        }
        else
        {
            //Load any saved data from a file using the data handler
            this.gameData = dataHandler.Load();
            //push the Loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            Debug.Log("Loaded local data");
        }

    }

    //Used by LoadGame when player is in webgl to get data from api
    IEnumerator GetAPIFileData(string fileName)
    {
        string url;
        if (PhotonNetwork.OfflineMode)
        {
            url = API_URL + $"/get_saved_game?fileName={fileName}&mode=Solo";
        }
        else
        {
            url = API_URL + $"/get_saved_game?fileName={fileName}&mode=Multiplayer";
        }

        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Error info: {www.error}");
        }
        else
        {
            // Procesa la respuesta
            string jsonResponse = www.downloadHandler.text;
            Debug.Log($"Respuesta recibida: {jsonResponse}");
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonResponse);
            //Load any saved data from a file using the API
            this.gameData = loadedData;
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            Debug.Log("Loaded API data");
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom) photonView.RPC("UpdateClientStatusToMaster", RpcTarget.MasterClient);
            //Called when data has loaded because of corutine use (only in solo player because players dont go to a lobby in there)
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && PhotonNetwork.OfflineMode) MenuUIController.instance.StartGame();
        }
    }

    //Called by photon when player joins room, sends data to load
    //Multiplayer games data is stored on MasterClient side
    public void PlayerJoined()
    {
        if (isNewGame)
        {
            photonView.RPC("LoadClientData", RpcTarget.Others, "", "vacio");
        }
        else
        {
            //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            //if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                photonView.RPC("LoadClientData", RpcTarget.Others, GameController.instance.fileSaveName, "webgl");
                StartCoroutine(GetAPIFileData(GameController.instance.fileSaveName));
            }
            else
            {
                LoadGame();
            }
        }
    }

    //Loads the data to client when they join the room
    //This rpc is called by master and the client runs it (it can either recieve the data in a string or the code of a file to call the API
    [PunRPC]
    public void LoadClientData(string dataString, string modo)
    {
        if(modo == "vacio")
        {
            NewGame();
            photonView.RPC("UpdateClientStatusToMaster", RpcTarget.MasterClient);
        }
        else if (modo == "webgl")
        {
            StartCoroutine(GetAPIFileData(dataString));
        }
        else
        {
            this.gameData = JsonUtility.FromJson<GameData>(dataString);
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            photonView.RPC("UpdateClientStatusToMaster", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void UpdateClientStatusToMaster()
    {
        MenuUIController.instance.clientHasLoadedSelfData = true;
    }

    //Calls all the SaveData functions in all scripts that implement it
    public void SaveGame(string APIFileCode, string password)
    {
        //pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        //Local data
        if (APIFileCode == "")
        {
            //save that data to a file using the data handler
            dataHandler.Save(gameData);
            Debug.Log("Saved local data");
        }
        //API data
        else
        {
            this.gameData.password = password;
            string jsonData = JsonUtility.ToJson(gameData);
            StartCoroutine(EnviarDatos(APIFileCode, jsonData));
        }
    }

    //Used by SaveGame when player is in webgl to save data to api
    //Note this api call asumes the API has started running already (theres no loading or safe loop)
    IEnumerator EnviarDatos(string fileName, string jsonData)
    {
        string url;
        if (PhotonNetwork.OfflineMode)
        {
            url = API_URL + $"/create_save_game?fileName={fileName}&mode=Solo";
        }
        else
        {
            url = API_URL + $"/create_save_game?fileName={fileName}&mode=Multiplayer";
        }
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
        UnityWebRequest request = new(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(jsonToSend),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error al enviar datos: {request.error}");
        }
        else
        {
            Debug.Log("Saved API data");
        }
        request.Dispose();
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
