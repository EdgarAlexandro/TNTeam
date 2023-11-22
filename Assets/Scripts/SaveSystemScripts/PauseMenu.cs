/* Function: controls the pause menu panel and buttons
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 05/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Realtime;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [Header("Pause Panel")]
    public GameObject pauseMenuWindow = null;
    public GameObject saveGameMenuButton = null;
    public GameObject exitGameMenuButton = null;
    public GameObject codeSpace = null;
    public TMP_InputField saveGameCode = null;
    public TMP_InputField passwordGameCode = null;
    public GameObject alertWindow = null;
    public GameObject loadingAlert = null;
    private bool callingAPI = false;
    private bool gameIsReseting = false;
    private readonly string API_URL = "https://apitnteam.edgar2208.repl.co";

    [Header("Player Canvas")]
    public GameObject playerCanvas = null;

    private void Awake()
    {
        pauseMenuWindow.SetActive(false);
        alertWindow.SetActive(false);
    }

    //RPC to open the pause menu to both players
    [PunRPC]
    public void OpenPauseMenu()
    {
        GameController.instance.isPaused = !GameController.instance.isPaused;
        playerCanvas.SetActive(!playerCanvas.activeSelf);
        pauseMenuWindow.SetActive(!pauseMenuWindow.activeSelf);
    }

    private void Update()
    {
        if (playerCanvas == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerCanvas = GameObject.Find("Canvas Player 1");
            }
            else
            {
                playerCanvas = GameObject.Find("Canvas Player 2");
            }
        }

        //The save system is only available to Masterclient
        if (!PhotonNetwork.IsMasterClient)
        {
            saveGameMenuButton.SetActive(false);
            codeSpace.SetActive(false);
        }
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (((saveGameCode.text.Length < 5 && GameController.instance.fileSaveName == "") || (passwordGameCode.text.Length < 5 && GameController.instance.password == "")) && Application.platform == RuntimePlatform.WebGLPlayer)
        //if (((saveGameCode.text.Length < 5 && GameController.instance.fileSaveName == "") || (passwordGameCode.text.Length < 5 && GameController.instance.password == "")) && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            saveGameMenuButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (!callingAPI) saveGameMenuButton.GetComponent<Button>().interactable = true;
        }
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (Application.platform != RuntimePlatform.WebGLPlayer || GameController.instance.fileSaveName != "")
        //if (Application.platform == RuntimePlatform.WebGLPlayer || GameController.instance.fileSaveName != "")
        {
            codeSpace.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            alertWindow.SetActive(false);
            TogglePauseMenu();
        }
    }

    //Calls the rpc that opens/closes the pause menu for all players in room
    public void TogglePauseMenu()
    {
        photonView.RPC("OpenPauseMenu", RpcTarget.All);
    }

    //Waits a certain amount of time for API to save data
    IEnumerator WaitForAPI()
    {
        yield return new WaitForSeconds(2);
        callingAPI = false;
        saveGameMenuButton.GetComponent<Button>().interactable = true;
    }

    //Starts an "Animation" in loading text
    private IEnumerator LoadingAnimation()
    {
        int dots = 0;
        loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text = "Loading";
        loadingAlert.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while (callingAPI)
        {
            if (dots < 3)
            {
                loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text += ".";
                dots++;
            }
            else
            {
                loadingAlert.GetComponentInChildren<TextMeshProUGUI>().text = "Loading";
                dots = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    //Used by Save button in pause menu, allows the user to save the game using the save system
    //WEBGL uses the API to store json files
    //MacOS and Windows uses the local file storage
    public void SaveGame()
    {
        //TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            callingAPI = true;
            saveGameMenuButton.GetComponent<Button>().interactable = false;
            if (GameController.instance.fileSaveName == "")
            {
                StartCoroutine(APIFileVerification(saveGameCode.text + ".json", passwordGameCode.text));
            }
            else
            {
                DataPersistenceManager.instance.SaveGame(GameController.instance.fileSaveName, GameController.instance.password);
                StartCoroutine(WaitForAPI());
            }
        }
        else
        {
            DataPersistenceManager.instance.SaveGame("", "");
        }

    }

    //Calls the API to check if a save with the given name can be created
    IEnumerator APIFileVerification(string fileName, string password)
    {
        string url;
        bool saveFileExists = false;
        if (PhotonNetwork.OfflineMode)
        {
            url = API_URL + $"/check_creation?fileName={fileName}&mode=Solo";
        }
        else
        {
            url = API_URL + $"/check_creation?fileName={fileName}&mode=Multiplayer";
        }
        bool apiCallWorks = false;
        StartCoroutine(LoadingAnimation());
        while (!apiCallWorks)
        {
            using UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                
                Debug.Log($"Error info: {www.error}");
                Debug.Log("La API probablemente esta apagada... starting API");
                yield return new WaitForSeconds(3);
            }
            else
            {
                Debug.Log($"Respuesta: {www.downloadHandler.text}");
                if (www.downloadHandler.text == "true")
                {
                    saveFileExists = true;
                }
                apiCallWorks = true;
            }
        }
        StopCoroutine(LoadingAnimation());
        loadingAlert.SetActive(false);
        if (saveFileExists)
        {
            alertWindow.SetActive(true);
            yield return new WaitForSeconds(4);
            alertWindow.SetActive(false);
        }
        else
        {
            GameController.instance.fileSaveName = fileName;
            GameController.instance.password = password;
            DataPersistenceManager.instance.SaveGame(fileName, password);
        }
        saveGameMenuButton.GetComponent<Button>().interactable = true;
        callingAPI = false;
    }

    //destroys all elements in game and shows a loading screen to reload the start menu
    [PunRPC]
    public void ResetGameAndLoadMainScene()
    {
        PersistenceManager.Instance.CleanAllInventories();
        gameIsReseting = true;
        pauseMenuWindow.SetActive(false);
        Destroy(GameObject.Find("EventSystem"));
        SceneManager.LoadScene("LoadingScreen");
        Destroy(PersistenceManager.Instance.gameObject);
        Destroy(DestructionManager.Instance.gameObject);
        Destroy(DropManager.Instance.gameObject);
        Destroy(DeathManager.Instance.gameObject);
        Destroy(DataPersistenceManager.instance.gameObject);
        Destroy(GameController.Instance.gameObject);
        Destroy(playerCanvas);
        Destroy(JokerSpawn.Instance.gameObject);
        Destroy(CardInventoryController.Instance.gameObject);
        Destroy(SpawnController.instance.gameObject);
        Destroy(GameObject.Find("PrefabsController"));
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<InventoryController>().inventory.items.Clear();
            Destroy(player);
        }
        Destroy(GameObject.Find("Canvas general(Clone)"));
        PhotonNetwork.Disconnect();
        Destroy(NetworkManager.instance.gameObject);
    }

    //In multiplayer, if a player leaves the room, sends the other one to the main menu
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!gameIsReseting) ResetGame();
    }

    //Calls the rpc that resets the game
    public void ResetGame()
    {
        photonView.RPC("ResetGameAndLoadMainScene", RpcTarget.All);
    }
}
