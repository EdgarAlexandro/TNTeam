using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardInventoryController : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryView;
    public List<Button> cardDisplayButtons;
    private List<CardData> inventoryCards;
    private bool isCardInventoryActive;

    // Start is called before the first frame update
    void Start()
    {
        isCardInventoryActive = false;
        inventoryCards = new List<CardData>();
        CardDisplay();
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
        }
    }

    public void CardDisplay(){
        inventoryCards.Clear();
        foreach (CardData card in inventory.cards){
            inventoryCards.Add(card);
        }

        for (int i = 0; i < inventoryCards.Count; i++){
            inventoryCards[i].prefabs[0].GetComponent<CardBehaviour>().SetCardFrontSprite();
            GameObject cardPrefab = inventoryCards[i].prefabs[0].gameObject;
            GameObject cardInstance = Instantiate(cardPrefab, cardDisplayButtons[i].transform.position, Quaternion.identity);
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = cardDisplayButtons[i].GetComponent<RectTransform>();
            cardRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
            cardInstance.transform.SetParent(cardDisplayButtons[i].transform);
            cardInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void AddCardToInventory(CardData selectedCardData)
    {
        if (inventory.cards.Count < inventory.maxCards)
        {
            inventory.AddCard(selectedCardData);
        }
    }
}
