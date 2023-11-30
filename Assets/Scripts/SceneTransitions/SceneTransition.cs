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

    bool TurnBasedCombatScene()
    {
        return sceneToLoad == "TurnBasedCombatTestScene";
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 lastPlayerPosition = nextPlayerPosition;
            if (PhotonNetwork.OfflineMode)
            {
                SceneManager.LoadScene(sceneToLoad);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = lastPlayerPosition;
            }
            //TODO En multiplayer no funciona correctamente el cambio de pos al entrar a escena (se ve como el otro player se transporta)
            else
            {
                playersNearby++;
                if (CanTransition())
                {
                    NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, sceneToLoad);

                    if (!TurnBasedCombatScene())
                    {
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                        //TODO En multiplayer no debe de mover al jugador si esta muerto
                        foreach (GameObject player in players)
                        {
                            if (player.GetPhotonView().IsMine)
                            {
                                player.transform.position = lastPlayerPosition;
                            }
                            else
                            {
                                player.GetPhotonView().RPC("MoveClient", RpcTarget.Others, lastPlayerPosition);
                            }
                            if (isNextSpawnVertical) lastPlayerPosition += new Vector2(1.2f, 0);
                            else lastPlayerPosition += new Vector2(0, 1.3f);
                        }
                    }
                }
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
