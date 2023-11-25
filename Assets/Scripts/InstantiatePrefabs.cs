/* Function: spawn prefabs of managers or controllers that need to be in every scene in case its the start scene
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstantiatePrefabs : MonoBehaviourPunCallbacks
{
    private static InstantiatePrefabs instance = null;
    public GameObject[] listOfPrefabs;
    public GameObject loadingScreen = null;
    public int seconds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            loadingScreen.SetActive(true);
            loadingScreen.GetComponent<Canvas>().sortingOrder = 100;
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        foreach (GameObject prefab in listOfPrefabs)
        {
            GameObject objeto;
            if (prefab.GetComponent<PhotonView>() == null)
            {
                objeto = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
                objeto.name = prefab.name;
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(prefab.name, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(seconds);
        loadingScreen.SetActive(false);
    }
}
