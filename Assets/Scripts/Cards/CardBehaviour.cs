using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public Sprite cardBack;
    public Sprite cardFront;
    private Image image;

    public void SetCardBackSprite()
    {
        image = GetComponent<Image>();
        if (image != null) // Check if the SpriteRenderer component is present
        {
            image.sprite = cardBack; // Set the sprite
        }
        else
        {
            Debug.LogError("Image component not found on this GameObject.");
        }
    }

    public void SetCardFrontSprite()
    {
        image = GetComponent<Image>();
        if (image != null) // Check if the SpriteRenderer component is present
        {
            image.sprite = cardFront; // Set the sprite
        }
        else
        {
            Debug.LogError("Image component not found on this GameObject.");
        }
    }

    public void ChangeCardsColor(Color color)
    {
        image = GetComponent<Image>();
        image.color = color;
    }

}
