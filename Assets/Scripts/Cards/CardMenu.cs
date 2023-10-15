/* Function: Display random cards inx the card menu and let the player select one
   Author: Daniel Degollado Rodríguez A008325555
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenu : MonoBehaviour
{
    // Card info class that includes a scriptable object called card data and ir's probability of appearence
    [System.Serializable]
    public class CardInfo{
        public CardData cardData;
        public float probability;
    }

    public List<CardInfo> cardInfoList;
    public List<Button> cardDisplay;
    public Button selectedCardDisplay;
    private GameObject selectedCard;
    public GameObject cardSelectMenu;
    public GameObject selectedCardView;
    public List<Button> selectedButton;
    public Inventory inventory;
    private List<CardData> randomCards;
    public CardData selectedCardData;
    public CardInventoryController cardInventoryController;

    // Start is called before the first frame update
    void Start(){
        randomCards = new List<CardData>();
        float totalProbability = 1f;
        //cardInventoryController = CardInventoryController.Instance;

        /*
        foreach (CardInfo cardInfo in cardInfoList)
        {
            totalProbability += cardInfo.probability;
        }
        */
        cardInventoryController = GameObject.Find("CardInventoryController").GetComponent<CardInventoryController>();
        for (int i = 0; i < 3; i++){
            // Get a random value
            float randomValue = Random.Range(0f, totalProbability);
            // Get list of shuffled cards
            List<CardInfo> cardInfoListShuffled = Shuffle(cardInfoList);
            // Set variable for the current probability of a card
            float currentProbability = 0f;
            // Boolean that checks if a card was added to the random cards list
            bool foundSuitableCard = false;
            foreach (CardInfo cardInfo in cardInfoListShuffled){

                if (!foundSuitableCard)
                { 
                    // Add the probability of the current card to the current probability variable
                    currentProbability += cardInfo.probability;
                    // If the tandom value is less or equal to the current probability, add the current card to random cards list
                    if (randomValue <= currentProbability)
                    {
                        randomCards.Add(cardInfo.cardData);
                        foundSuitableCard = true;
                    }
                }
            }
        }
        CardDisplay(randomCards);
    }
    // Shuffle the lists of available cards. Takes a list of card info objects as a parameter.
    public static List<CardInfo> Shuffle<CardInfo>(List<CardInfo> list){
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            CardInfo value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    // Display the cards in the card menu. Takes a list of cards as a parameter.
    void CardDisplay(List<CardData> cards){
        for (int i = 0; i < cards.Count; i++){
            // Sets sprite of card to the back one.
            cards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardBackSprite();
            // Instantiate prefab in the CardData scriptable object 
            GameObject cardInstance = Instantiate(cards[i].prefabs[0], cardDisplay[i].transform.position, Quaternion.identity);
            // Modify prefab size
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = cardDisplay[i].GetComponent<RectTransform>();
            cardRectTransform.sizeDelta = buttonRectTransform.sizeDelta;                   
            // Define parent and local position of prefab
            cardInstance.transform.SetParent(cardDisplay[i].transform);
            cardInstance.transform.localPosition = Vector3.zero;
        }
    }

    // Function asigned to a button. Select the card that is the children of the clicked button. Takes card index as a parameter.
    public void SelectCard(int cardIndex){
        // Color variables to display if a card is clicked or not
        Color selectedColor = new Color(0.4f, 0.9f, 0.7f, 1.0f);
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        // Status variable used to determine if a card is clicked or not
        bool status = false;
        // If there is a selected card
        if (selectedButton.Count > 0){
            // Deselects the card and selects the new one
            DeselectPreviousCard(normalColor);
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
        // If there is not
        else{
            // Selects a card
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
        Debug.Log(selectedCard);
    }

    // Function assigned to a button. Displays selected card.
    public void OnAcceptCardClick(){
        // Change color of card to normal
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        Debug.Log(selectedCardData);
        // If there is a selected card
        if (selectedCard != null){
            // Adds card to inventory and adjusts it for the selected card view
            AddCardToInventory();
            // Changes appearance of card
            selectedCard.GetComponent<CardBehaviour>().SetCardFrontSprite();
            selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
            // Disables view of card menu and enables view of selected card
            cardSelectMenu.SetActive(false);
            selectedCardView.SetActive(true);
            // Changes position of selected card
            selectedCard.transform.SetParent(selectedCardDisplay.transform);
            selectedCard.transform.localPosition = Vector3.zero;
            // Reset selected card variable
            selectedCard = null; 
        }
    }

    // Function asigned to button. Disables the view of the selected card and returns to normal gameplay.
    public void OnContinueClick(){
        // Gets all active jokers in the current scene (possible change to a better way)
        GameObject[] activeJoker;
        activeJoker = GameObject.FindGameObjectsWithTag("Joker");
        //Disables the view of the selected card
        selectedCardView.SetActive(false);
        // Destroys all active jokers
        foreach (GameObject joker in activeJoker){
            Destroy(joker);
        }
    }

    //Adds card to inventory.
    public void AddCardToInventory(){
        // Uses functions from card Inventory Controller
        cardInventoryController.AddCardToInventory(selectedCardData);
        cardInventoryController.CardDisplay();
    }

    // If there is a selected card, it deselects it. Takes color as a parameter.
    private void DeselectPreviousCard(Color normalColor){
        // Gets the ButtonHoverColorChange component from the selected card and sets is status to false (not selected)
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(false);
        // Gets the prefab set as a child of the selected button and changes it's color back to normal to show it's not selected anymore
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;   
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
        // Clears selected button list
        selectedButton.Clear();  
    }

    // Select a card. Takes the index of the card, selected color and a boolean status variable as parameters.
    private void SelectNewCard(int cardIndex, Color selectedColor, ref bool status){
        // Adds the clicked button to the selected button list 
        selectedButton.Add(cardDisplay[cardIndex]);
        // Gets the CardButton component from the selected button
        CardButton cardButton = selectedButton[0].gameObject.GetComponent<CardButton>();
        if (cardButton != null){
            // Sets the associated card variable as the card with the same index as the button
            cardButton.associatedCard = randomCards[cardIndex];
            selectedCardData = cardButton.associatedCard;
        }
        // Gets the prefab set as a child of the selected button and changes it's color back to normal to show it's selected 
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(selectedColor);
        // Gets the ButtonHoverColorChange component from the selected card and sets is status to true (selected)
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(true);
    }
}
