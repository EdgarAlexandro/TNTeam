using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenu : MonoBehaviour
{

    [System.Serializable]
    public class CardInfo
    {
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

    //public GameObject[] cardPrefabs;
    //public GameObject cardDisplay;
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
            float randomValue = Random.Range(0f, totalProbability);
            List<CardInfo> cardInfoListShuffled = Shuffle(cardInfoList);
            float currentProbability = 0f;
            bool foundSuitableCard = false;
            foreach (CardInfo cardInfo in cardInfoListShuffled){

                if (!foundSuitableCard)
                {
                    currentProbability += cardInfo.probability;

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

    void CardDisplay(List<CardData> cards){
        for (int i = 0; i < cards.Count; i++){

            cards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardBackSprite();
            //GameObject cardPrefab = cards[i].prefabs[i];
            // Instancia prefab ubicado en el scriptable object
            GameObject cardInstance = Instantiate(cards[i].prefabs[0], cardDisplay[i].transform.position, Quaternion.identity);
            
            // Modifica tama?o de imagen
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = cardDisplay[i].GetComponent<RectTransform>();
            cardRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
            
            // Define padre  y posici?n local
            cardInstance.transform.SetParent(cardDisplay[i].transform);
            //cards[i].SetParent(cardDisplay[i].transform);
            cardInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void SelectCard(int cardIndex){
        Color selectedColor = new Color(0.4f, 0.9f, 0.7f, 1.0f);
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        bool status = false;
        if (selectedButton.Count > 0){
            DeselectPreviousCard(normalColor);
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
        else{
            SelectNewCard(cardIndex, selectedColor, ref status);
        }
       
        //cardDisplay[cardIndex].GetComponent<ButtonHoverColorChange>().CardStatus();
        Debug.Log(selectedCard);
    }

    public void OnAcceptCardClick(){
        Color normalColor = new Color(255f, 255f, 255f, 255f);
        Debug.Log(selectedCardData);
        if (selectedCard != null){
            AddCardToInventory(); 
            selectedCard.GetComponent<CardBehaviour>().SetCardFrontSprite();
            selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
            cardSelectMenu.SetActive(false);
            selectedCardView.SetActive(true);
            selectedCard.transform.SetParent(selectedCardDisplay.transform);
            selectedCard.transform.localPosition = Vector3.zero;
            selectedCard = null; // Reset selected card
            
        }
    }

    public void OnContinueClick()
    {
        GameObject[] activeJoker;
        activeJoker = GameObject.FindGameObjectsWithTag("Joker");
        selectedCardView.SetActive(false);
        foreach (GameObject joker in activeJoker)
        {
            Destroy(joker);
        }
    }

    public void AddCardToInventory()
    {
        /*if(inventory.cards.Count < inventory.maxCards)
        {
            inventory.AddCard(selectedCardData);
        }*/
        cardInventoryController.AddCardToInventory(selectedCardData);
        cardInventoryController.CardDisplay();
    }

    private void DeselectPreviousCard(Color normalColor){
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(false);
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        //GameObject selectedCardPrefab = selectedCard.prefabs[0];
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
        //selectedCard.prefabs[0].GetComponent<CardBehaviour>().ChangeCardsColor(normalColor);
        selectedButton.Clear();  
    }

    private void SelectNewCard(int cardIndex, Color selectedColor, ref bool status){
        selectedButton.Add(cardDisplay[cardIndex]);
        //selectedCard = cardDisplay[cardIndex].gameObject.transform.GetChild(0);
        CardButton cardButton = selectedButton[0].gameObject.GetComponent<CardButton>();
        if (cardButton != null)
        {
            cardButton.associatedCard = randomCards[cardIndex];
            selectedCardData = cardButton.associatedCard;
        }
    
        selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        
        //GameObject selectedCardPrefab = selectedCard.prefabs[0];
        //selectedCard = selectedButton[0].gameObject.transform.GetChild(0).gameObject;
        selectedCard.GetComponent<CardBehaviour>().ChangeCardsColor(selectedColor);
        status = true;
        //cardDisplay[cardIndex].GetComponent<ButtonHoverColorChange>().CardStatus(status);
        selectedButton[0].GetComponent<ButtonHoverColorChange>().CardStatus(status);
    }
}
