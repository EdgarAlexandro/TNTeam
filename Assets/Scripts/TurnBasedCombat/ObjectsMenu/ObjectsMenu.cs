using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Photon.Pun;

public class ObjectsMenu : MonoBehaviourPunCallbacks
{
    public List<GameObject> players;
    public List<GameObject> objectButtons;
    public GameObject bomba;
    public Transform bombaSpawn;
    public GameObject currentCharacter;
    //private GameObject playerGameObject;
    //private Inventory inventory;

    private GameObject healthBars;

    private PersistenceManager pm;
    private TurnBasedCombatPlayersHealth tbcPH;
    private TurnBasedCombatPlayersMagic tbcPM;

    // Start is called before the first frame update
    void Start()
    {
        pm = PersistenceManager.Instance;
        healthBars = GameObject.Find("HealthBars");
        tbcPH = healthBars.GetComponent<TurnBasedCombatPlayersHealth>();
        tbcPM = healthBars.GetComponent<TurnBasedCombatPlayersMagic>();
        CountAvailableObjects();
        StartCoroutine("GetCurrentPlayer");
    }

    IEnumerator GetCurrentPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                currentCharacter = player;
            }
        }
    }

    [PunRPC]
    void ReviveAnimation(string characterName)
    {
        GameObject characterToAnimate = GameObject.Find(characterName);
        characterToAnimate.GetComponent<Animator>().SetBool("Muerte", false);
    }

    void CountAvailableObjects()
    {
        string inventoryData = (string)PhotonNetwork.LocalPlayer.CustomProperties["Inventory"];
        foreach (GameObject button in objectButtons)
        {
            MatchCollection matches = Regex.Matches(inventoryData, button.name);
            int itemCount = matches.Count;
            button.gameObject.GetComponent<ObjectsMenuButton>().GetNumberOfUses(itemCount);
        }
    }

    [PunRPC]
    public void PocionCuracion()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentHealth + 10 < tbcPH.p1HealthBar.maxValue)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPH.p1HealthBar.maxValue - pm.CurrentHealth;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
        }
        else
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentHealth + 10 < tbcPH.p2HealthBar.maxValue)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPH.p1HealthBar.maxValue - pm.CurrentHealth;
                tbcPH.photonView.RPC("SyncronizeP2HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
            }
        }
    }

    [PunRPC]
    public void PocionMagia()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentMagic + 15 < tbcPM.p1MagicBar.maxValue)
            {
                pm.CurrentMagic += 15;
                tbcPM.photonView.RPC("SyncronizeP1MagicBarCurrentValue", RpcTarget.All, pm.CurrentMagic);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPM.p1MagicBar.maxValue - pm.CurrentMagic;
                tbcPM.photonView.RPC("SyncronizePMagicBarCurrentValue", RpcTarget.All, pm.CurrentMagic);
            }
        }
        else
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentMagic + 15 < tbcPM.p2MagicBar.maxValue)
            {
                pm.CurrentMagic += 15;
                tbcPM.photonView.RPC("SyncronizeP2MagicBarCurrentValue", RpcTarget.All, pm.CurrentMagic);
            }
            else
            {
                pm.CurrentHealth += (int)tbcPM.p2MagicBar.maxValue - pm.CurrentMagic;
                tbcPM.photonView.RPC("SyncronizeP2MagicBarCurrentValue", RpcTarget.All, pm.CurrentMagic);
            }
        }
    }

    [PunRPC]
    public void PocionRevivir()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentHealth <= 0)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                currentCharacter.GetComponent<TurnBasedCombatPlayerDeath>().photonView.RPC("IncreaseAlivePlayers", RpcTarget.All);
                photonView.RPC("ReviveAnimation", RpcTarget.All, currentCharacter.name);
            }
        }
        else
        {
            currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
            currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
            if (pm.CurrentHealth <= 0)
            {
                pm.CurrentHealth += 10;
                tbcPH.photonView.RPC("SyncronizeP1HealthBarCurrentValue", RpcTarget.All, pm.CurrentHealth);
                currentCharacter.GetComponent<TurnBasedCombatPlayerDeath>().photonView.RPC("IncreaseAlivePlayers", RpcTarget.All);
                photonView.RPC("ReviveAnimation", RpcTarget.All, currentCharacter.name);
            }
        }
    }

    public void Bomba()
    {
        currentCharacter.GetComponent<MenuControllerCBT>().canControl = false;
        currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
        PhotonNetwork.Instantiate(bomba.name, bombaSpawn.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
