/* Function: Create CardData scriptable objects
   Author: Daniel Degollado Rodríguez A008325555
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject{
    // Name of the card
    public string cardName;
    // Prefab of the card
    public GameObject[] prefabs;
}
