/* Function: creates a scriptableObject-derived where each item data is stored
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ItemData : ScriptableObject
{
    [TextArea]
    public string description = null;
    public string itemName = null;
    public string[] prefabs = null;
    public Sprite itemImage = null;
}