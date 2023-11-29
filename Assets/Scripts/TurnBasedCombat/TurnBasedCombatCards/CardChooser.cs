using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}



public class CardChooser : MonoBehaviour
{

    CardInventoryController cardInventoryController;
    TurnBasedCombatManager tbcm;
    public TurnBasedCardActions tbca;
    public Inventory inventory;
    List<CardData> playerCards;
    private int selectedCardIndex;

    public List<Button> cardDisplay; // lista con los botones de las cartas
    public List<Button> selectedButton; // para saber que carta esta seleccionada

    public GameObject cardSelectMenu; // panel de seleccionar cartas

    private GameObject selectedCard; // aqui se almacena la carta seleccionada
    public CardData selectedCardData; // la info de la carta seleccionada

    public GameObject selectedCardView; // panel de la carta seleccionada

    public Button selectedCardDisplay; // el gameobject donde se va a desplegar la carta seleccionada

    //void Awake()
    //{
    //    tbcm = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>();
    //    tbca = GetComponent<TurnBasedCardActions>();
    //    cardInventoryController = GameObject.Find("CardInventoryController").GetComponent<CardInventoryController>();
    //    playerCards = inventory.cards;
    //    cardSelectMenu = GameObject.Find("CardChooserPanel");
    //    playerCards.Shuffle();

        
    //}

    // Start is called before the first frame update
    void Start()
    {
        tbcm = GameObject.Find("TurnBasedCombatManager").GetComponent<TurnBasedCombatManager>();
        tbca = GetComponent<TurnBasedCardActions>();
        cardInventoryController = GameObject.Find("CardInventoryController").GetComponent<CardInventoryController>();
        
    }

    private void Awake()
    {
        playerCards = inventory.cards;
        playerCards.Shuffle();

        CardDisplay(playerCards);
        cardSelectMenu.SetActive(true);
        selectedCardView.SetActive(false);
    }

    void CardDisplay(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (i < cardDisplay.Count)
            {
                // Sets sprite of card to the back one.
                cards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardBackSprite();
                // Instantiate prefab in the CardData scriptable object 
                GameObject cardInstance = Instantiate(cards[i].prefabs[1], cardDisplay[i].transform.position, Quaternion.identity);
                
                // Modify prefab size
                //RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
                //RectTransform buttonRectTransform = cardDisplay[i].GetComponent<RectTransform>();
                //cardRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
                // Define parent and local position of prefab
                cardInstance.transform.SetParent(cardDisplay[i].transform);
                cardInstance.transform.localScale = new Vector3(1, 1, 1);
                cardInstance.transform.localPosition = Vector3.zero;
            }
            
        }

        for (int i = 0; i < 3; i++)
        {
            if (i <= cards.Count - 1)
            {
                // Fijamos la sprite 
                cards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardBackSprite();
            }
        }
    }

    void UseCard(int cardIndex)
    {
        string cardName = playerCards[cardIndex].cardName;
        CardEffect(cardName);
        playerCards.RemoveAt(cardIndex);
        cardInventoryController.UpdateCardInventory();
    }

    void CardEffect(string cardName)
    {
        switch (cardName)
        {
            case "Confusion":
                //bla
                tbca.Confusion();
                break;

            case "HeyNotYet":
                tbca.NotYet();
                break;

            case "JokersPrank":
                tbca.JokersPrank();
                break;

            case "SanaSanaColitaDeRana":
                tbca.SanaSana();
                break;

            case "TheCentinels":
                tbca.Sentinel();
                break;

            case "MayTheForceBeWithYou":
                tbca.MayTheForceBeWithYou();
                break;

            default:
                break;

        }
    }

    public void SelectCard(int cardIndex)
    {
        selectedCardIndex = cardIndex;
        // Color variables to display if a card is clicked or not
        Color selectedColor = new Color(0.4f, 0.9f, 0.7f, 1.0f);
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        // Status variable used to determine if a card is clicked or not
        bool status = false;
        // If there is a selected card
        if (selectedButton.Count > 0)
        {
            // Deselects the card and selects the new one
            DeselectPreviousCard(normalColor);
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
        // If there is not
        else
        {
            // Selects a card
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
        Debug.Log(selectedCard);
    }

    // If there is a selected card, it deselects it. Takes color as a parameter.
    private void DeselectPreviousCard(Color normalColor)
    {
        // Gets the ButtonHoverColorChange component from the selected card and sets is status to false (not selected)
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(false);
        // Gets the prefab set as a child of the selected button and changes it's color back to normal to show it's not selected anymore
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
        // Clears selected button list
        selectedButton.Clear();
    }

    // Select a card. Takes the index of the card, selected color and a boolean status variable as parameters.
    private void SelectNewCard(int cardIndex, Color selectedColor, ref bool status)
    {
        selectedCardIndex = cardIndex;
        // Adds the clicked button to the selected button list 
        selectedButton.Add(cardDisplay[cardIndex]);
        // Gets the CardButton component from the selected button
        CardButton cardButton = selectedButton[0].gameObject.GetComponent<CardButton>();
        if (cardButton != null)
        {
            // Sets the associated card variable as the card with the same index as the button
            cardButton.associatedCard = playerCards[cardIndex];
            selectedCardData = cardButton.associatedCard;
        }
        // Gets the prefab set as a child of the selected button and changes it's color back to normal to show it's selected 
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(selectedColor);
        // Gets the ButtonHoverColorChange component from the selected card and sets is status to true (selected)
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(true);
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

    // Button
    // Functions

    // When you click on continue after seeing the card you chose
    public void OnContinueClick()
    {
        // uses the selected card
        UseCard(selectedCardIndex);

        cardSelectMenu.SetActive(true);
        selectedCardView.SetActive(false);

        tbcm.EndTurn();
        gameObject.SetActive(false);
    }

    // Function assigned to a button. Displays selected card.
    public void OnAcceptCardClick()
    {
        // Change color of card to normal
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        Debug.Log("Selected card: " + selectedCard.name);
        // If there is a selected card
        if (selectedCard != null)
        {
            // Changes appearance of card
            selectedCard.GetComponent<CardBehaviour>().SetCardFrontSprite();
            selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
            // // Disables view of card menu and enables view of selected card
            cardSelectMenu.SetActive(false);
            selectedCardView.SetActive(true);
            // // Changes position of selected card
            selectedCard.transform.SetParent(selectedCardDisplay.transform);
            selectedCard.transform.localPosition = Vector3.zero;
            // Reset selected card variable
            selectedCard = null;
        }
    }

    public void OnExitBtnClick()
    {
        gameObject.SetActive(false);
    }
}




// Regex.matches(inventorydata, button.name)