using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Photon.Pun;

public class ObjectsMenuButton : MonoBehaviourPunCallbacks
{
    public Button button;
    public GameObject objectsMenu;
    public GameObject objectsMenuContainer;
    public TextMeshProUGUI itemCountText;
    public int uses;

    private TurnBasedCombatManager tbc;

    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
    }

    public void GetNumberOfUses(int numberOfUses)
    {
        uses = numberOfUses;
        itemCountText.text = uses.ToString();
        UpdateButtonStatus();
    }

    void UpdateButtonStatus()
    {
        if(uses > 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void PocionCuracionButton()
    {
        if(uses > 0)
        {
            uses--;
            itemCountText.text = uses.ToString();
            objectsMenu.GetComponent<ObjectsMenu>().photonView.RPC("PocionCuracion", RpcTarget.All);
            UpdateButtonStatus();
            objectsMenuContainer.SetActive(false);
            tbc.EndTurn();
        }
    }

    public void PocionMagiaButton()
    {
        if(uses > 0)
        {
            uses--;
            itemCountText.text = uses.ToString();
            objectsMenu.GetComponent<ObjectsMenu>().photonView.RPC("PocionMagia", RpcTarget.All);
            UpdateButtonStatus();
            objectsMenuContainer.SetActive(false);
            tbc.EndTurn();
        }
    }

    public void PocionRevivirButton()
    {
        if(uses > 0)
        {
            uses--;
            itemCountText.text = uses.ToString();
            objectsMenu.GetComponent<ObjectsMenu>().photonView.RPC("PocionRevivir", RpcTarget.All);
            UpdateButtonStatus();
            objectsMenuContainer.SetActive(false);
            tbc.EndTurn();
        }
    }

    public void BombaButton()
    {
        if(uses > 0)
        {
            uses--;
            itemCountText.text = uses.ToString();
            objectsMenu.GetComponent<ObjectsMenu>().Bomba();
            UpdateButtonStatus();
            objectsMenuContainer.SetActive(false);
            tbc.canvas.SetActive(false);
        }
    }

    public void ExitButton()
    {
        GameObject currentCharacter = objectsMenu.GetComponent<ObjectsMenu>().currentCharacter;
        currentCharacter.GetComponent<MenuControllerCBT>().canControl = true;
        currentCharacter.GetComponent<MenuControllerCBT>().objectsMenuActive = false;
        objectsMenuContainer.SetActive(false);
    }

    /*public void UpdateInventory()
    {
        string data = "";
        foreach (ItemData item in inventory.items)
        {
            data += item.name + "/";
        }
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties["Inventory"] = data;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
