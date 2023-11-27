using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TBCLoadingScene : MonoBehaviourPunCallbacks
{
    public GameObject loadingScreen = null;
    private int seconds = 6;

    private void Awake()
    {
        if (PhotonNetwork.OfflineMode)
        {
            seconds = 1;
        }
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<Canvas>().sortingOrder = 100;
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(seconds);
        loadingScreen.SetActive(false);
    }
}
