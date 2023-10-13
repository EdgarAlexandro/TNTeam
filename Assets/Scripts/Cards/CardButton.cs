using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour{
    public CardData associatedCard;

    public void setCardData(CardData cardData){
        associatedCard = cardData;
    }
}