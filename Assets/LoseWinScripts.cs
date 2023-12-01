using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseWinScripts : MonoBehaviour
{
    public void ResetGameAndLoadMainScene()
    {
        PersistenceManager.Instance.CleanAllInventories();
        Destroy(GameObject.Find("EventSystem"));
        SceneManager.LoadScene("LoadingScreen");
        Destroy(PersistenceManager.Instance.gameObject);
        Destroy(DestructionManager.Instance.gameObject);
        Destroy(DropManager.Instance.gameObject);
        Destroy(DeathManager.Instance.gameObject);
        Destroy(DataPersistenceManager.instance.gameObject);
        Destroy(GameController.Instance.gameObject);
        Destroy(JokerSpawn.Instance.gameObject);
        Destroy(CardInventoryController.Instance.gameObject);
        Destroy(SpawnController.instance.gameObject);
        Destroy(GameObject.Find("PrefabsController"));
        Destroy(GameObject.Find("TurnBasedCombatManager"));
        
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<InventoryController>().inventory.items.Clear();
            Destroy(player);
        }
        PhotonNetwork.Disconnect();
        Destroy(NetworkManager.instance.gameObject);
    }
}
