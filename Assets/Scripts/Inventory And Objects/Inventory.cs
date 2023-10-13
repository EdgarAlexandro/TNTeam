using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public int maxItems = 10;
    public int maxCards = 6;
    public List<ItemData> items = new();
    public List<CardData> cards = new();

    public bool AddItem(ItemData itemToAdd)
    {
        items.Add(itemToAdd);
        return true;
    }

    public bool AddCard(CardData cardToAdd)
    {
        cards.Add(cardToAdd);
        return true;
    }

    public bool DropItem(int indexItemToDrop, Vector3 position)
    {
        Instantiate(items[indexItemToDrop].prefabs[0], position, Quaternion.identity);
        items.RemoveAt(indexItemToDrop);
        return false;
    }

    public bool UseItem(int indexOfItem)
    {
        items.RemoveAt(indexOfItem);
        return true;
    }

    public void RemoveAllItems()
    {
        items.Clear();
    }
}