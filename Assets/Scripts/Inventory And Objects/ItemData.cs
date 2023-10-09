using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject[] prefabs;
    [TextArea]
    public string description;
    public Sprite itemImage;
}