using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuertas : MonoBehaviour
{

    public int neededKeys;
    //public GameObject sceneTransitionPrefab;
    private SpriteRenderer sprite;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor(int keyAmount)
    {
        if(keyAmount == neededKeys)
        {
            // Instantiate the scene transition prefab
            //Instantiate(sceneTransitionPrefab, transform.position, Quaternion.identity);

            // Disable the collider of the door
            GetComponent<Collider2D>().enabled = false;

            //Set color of door to black to indicate that its open
            sprite.color = new Color(0, 0, 0, 255);
        }
    }
    



    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<UIController>().currentKeys >= (1))
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<UIController>().increaseKeyCount(-1);
            }
        }
    }
    */

}
