using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuertas : MonoBehaviour
{
    public int neededKeys;
    //public GameObject sceneTransitionPrefab;
    private SpriteRenderer sprite;
    private PersistenceManager pm;
    

    void Start()
    {
        pm = PersistenceManager.Instance;
        sprite = GetComponent<SpriteRenderer>();
        // If player has the amount of needed keys, perform logic for this case
        if(pm.CurrentKeys == neededKeys)
        {
            OnNeededKeysCollected();
        }

    }

    void Update()
    {
        // If player has the amount of needed keys, perform logic for this case
        if (pm.CurrentKeys == neededKeys)
        {
            OnNeededKeysCollected();
        }

    }
    // Logic when amount of needed keys collected
    void OnNeededKeysCollected()
    {
        // Disable the collider of the door
        GetComponent<Collider2D>().enabled = false;

        //Set color of door to black to indicate that its open
        sprite.color = new Color(0, 0, 0, 255);
    }

}
