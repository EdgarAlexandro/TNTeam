/* Function: Control the inventory, manage its display and player interaction 
   Author: Daniel Degollado Rodríguez A008325555
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardInventoryController : MonoBehaviourPunCallbacks
{
    public Inventory inventory;
    public GameObject inventoryView;
    public List<Button> cardDisplayButtons;
    private List<CardData> inventoryCards;
    private bool isCardInventoryActive;
    //private UIController uiController;

    private static CardInventoryController instance;
    public static CardInventoryController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start(){
        isCardInventoryActive = false;
        inventoryCards = new List<CardData>();
        //inventoryView = uiController.playerCanvas.transform.Find("CardInventory").gameObject;
        //CardDisplay();
        UpdateCardInventory();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.C)){
            if (!isCardInventoryActive){
                isCardInventoryActive = true;
                inventoryView.SetActive(true);
            }
            else{
                inventoryView.SetActive(false);
                isCardInventoryActive = false;
            }
            CardDisplay();
        }
    }

    // Display the selected cards inside the inventory
    public void CardDisplay(){
        // Clear the list used to display the cards so they dont appear duplicated in the inventory
        inventoryCards.Clear();
        foreach (CardData card in inventory.cards){
            inventoryCards.Add(card);
        }

        for (int i = 0; i < inventoryCards.Count; i++){
            // Change the image of the card to the front one
            inventoryCards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardFrontSprite();
            // Get the prefab of the card
            GameObject cardPrefab = inventoryCards[i].prefabs[0].gameObject;
            // Instantiate the prefab
            GameObject cardInstance = Instantiate(cardPrefab, cardDisplayButtons[i].transform.position, Quaternion.identity);
            // Adjust the size of the card to the one of the button it's displayed on
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = cardDisplayButtons[i].GetComponent<RectTransform>();
            cardRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
            // Adjust the position of the card to the one ot the button it's displayed on
            cardInstance.transform.SetParent(cardDisplayButtons[i].transform);
            cardInstance.transform.localPosition = Vector3.zero;
        }
    }

    // Add the selected card to the inventory. Takes an object of  CardData type as a parameter.
    public void AddCardToInventory(CardData selectedCardData){
        // Check if the inventory hasn't reached it's limit
        if (inventory.cards.Count < inventory.maxCards){
            inventory.AddCard(selectedCardData);
            UpdateCardInventory();
        }
    }

    //Used by both players but important for client (not master)
    //This function saves the names of each card (currently in inventory) in the custom properties (Photon) of the player 
    public void UpdateCardInventory()
    {
        string data = "";
        foreach (CardData card in inventory.cards)
        {
            data += card.name + "/";
        }
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties["Cards"] = data;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
}
