using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Photon.Pun;

public class ObjectsMenu : MonoBehaviourPunCallbacks
{
    public List<PlayerInNetwork> players;
    public List<GameObject> objectButtons;
    private TurnBasedCombatManager tbc;
    private GameObject playerGameObject;
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        CountAvailableObjects();
        //StartCoroutine("AssignPlayers");
    }

    void CountAvailableObjects()
    {
        string inventoryData = (string)PhotonNetwork.LocalPlayer.CustomProperties["Inventory"];
        foreach (GameObject button in objectButtons)
        {
            MatchCollection matches = Regex.Matches(inventoryData, button.name);
            int itemCount = matches.Count;
            button.gameObject.GetComponent<ObjectsMenuButton>().GetNumberOfUses(itemCount);
            Debug.Log($"{button.name} count: {itemCount}");
        }
        Debug.Log(inventoryData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
