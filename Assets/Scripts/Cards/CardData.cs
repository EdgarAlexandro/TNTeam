using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string cardName;
    public GameObject[] prefabs;
    public Sprite cardBackImage;
    public Sprite cardFrontImage;
}
