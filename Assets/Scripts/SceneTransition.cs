using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneTransition : MonoBehaviourPunCallbacks
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public int playersNearby = 0;

    public bool CanTransition()
    {
        return playersNearby == GameController.AlivePlayers;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (PhotonNetwork.OfflineMode)
            {
                playerStorage.initialValue = playerPosition;
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                playersNearby++;
                Debug.Log(playersNearby);
                if (CanTransition())
                {
                    NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, sceneToLoad);
                    GameController.instance.hasPlayersSpawned = false;
                }
            }
            //playerStorage.initialValue = playerPosition;
            //NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, sceneToLoad);
            //SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PhotonNetwork.OfflineMode)
            {
                playersNearby--;
                Debug.Log(playersNearby);
            }
        }
    }
}
