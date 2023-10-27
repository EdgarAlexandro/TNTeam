/* Function: Activate the card menu when the player interacts with the Joker
   Author: Daniel Degollado Rodríguez 
   Modification date: 14/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class JokerCards : MonoBehaviourPunCallbacks
{
    public GameObject cardMenu; 
    private Vector2 objectPosition;
    private string currentSceneName;
    private DropManager drm;

    void Start(){
        drm = DropManager.Instance;
        objectPosition = transform.position;
        currentSceneName = SceneManager.GetActiveScene().name;
        tag = gameObject.tag;

        // When the object appears, add its position to the drop position list located in the drop manager
        //drm.AddDropPosition(objectPosition, currentSceneName, tag);
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Player")
        {
            GameObject playerCanvas = other.gameObject.GetComponent<UIController>().playerCanvas;
            cardMenu = playerCanvas.transform.Find("CardMenu").gameObject;
            cardMenu.SetActive(true);
        }
    }
}
