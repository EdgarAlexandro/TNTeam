/* Function: controls the pause menu panel and buttons
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 05/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [Header("Pause Panel")]
    public GameObject pauseMenuWindow = null;
    public GameObject saveGameMenuButton = null;
    public GameObject exitGameMenuButton = null;

    [Header("Player Canvas")]
    public GameObject playerCanvas = null;

    private void Awake()
    {
        pauseMenuWindow.SetActive(false);
    }

    //RPC to open the pause menu to both players
    [PunRPC]
    public void OpenPauseMenu()
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
        GameController.instance.isPaused = !GameController.instance.isPaused;
        playerCanvas.SetActive(!playerCanvas.activeSelf);
        pauseMenuWindow.SetActive(!pauseMenuWindow.activeSelf);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            saveGameMenuButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            photonView.RPC("OpenPauseMenu", RpcTarget.All);
        }
    }

    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
