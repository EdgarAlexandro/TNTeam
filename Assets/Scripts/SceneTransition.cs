using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneTransition : MonoBehaviourPun
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerStorage.initialValue = playerPosition;
            NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, sceneToLoad);
            //SceneManager.LoadScene(sceneToLoad);
        }
    }
}
