using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneTransition : MonoBehaviourPunCallbacks
{
    public string sceneToLoad;
    public Vector2 nextPlayerPosition;
    public int playersNearby = 0;
    public bool isNextSpawnVertical = false;


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
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                playersNearby++;
                if (CanTransition())
                {
                    NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, sceneToLoad);
                }
            }
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Vector2 lastPlayerPosition = nextPlayerPosition;

            foreach (GameObject player in players)
            {
                player.transform.position = lastPlayerPosition;
                if (isNextSpawnVertical) lastPlayerPosition += new Vector2(1, 0);
                else lastPlayerPosition += new Vector2(0, 1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PhotonNetwork.OfflineMode)
            {
                playersNearby--;
            }
        }
    }
}
