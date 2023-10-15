/* Function: Associate a card with a button once it is clicked
   Author: Daniel Degollado Rodríguez A008325555
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour{
    public CardData associatedCard;

    // Sets associated card to the card received. Takes a CardData scriptable object as a parameter.
    public void setCardData(CardData cardData){
        associatedCard = cardData;
    }
}