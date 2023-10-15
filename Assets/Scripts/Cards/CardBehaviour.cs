/* Function: Manage behaviour of the cards
   Author: Daniel Degollado Rodríguez A008325555
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public Sprite cardBack;
    public Sprite cardFront;
    private Image image;


    // Sets's card sprite as the back one
    public void SetCardBackSprite(){
        image = GetComponent<Image>();
        // Check if the SpriteRenderer component is present
        if (image != null) {
            // Set the sprite
            image.sprite = cardBack; 
        }
        else{
            Debug.LogError("Image component not found on this GameObject.");
        }
    }

    // Sets card's sprite as the front one
    public void SetCardFrontSprite(){
        image = GetComponent<Image>();
        // Check if the SpriteRenderer component is present
        if (image != null) {
            // Set the sprite
            image.sprite = cardFront; 
        }
        else{
            Debug.LogError("Image component not found on this GameObject.");
        }
    }

    // Change the color of the card
    public void ChangeCardsColor(Color color){
        image = GetComponent<Image>();
        image.color = color;
    }

}
