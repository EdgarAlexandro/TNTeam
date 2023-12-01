using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class CharacterDeactivation : MonoBehaviourPunCallbacks
{

    public List<GameObject> playerPrefabs = new List<GameObject>();
    public List<GameObject> playerGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DeActivateBeatEmUpCharacters();
        DeActivateCharactersCanvas();
    }

    private void DeActivateBeatEmUpCharacters()
    {
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject player in playerGameObjects)
        {
            foreach (GameObject prefab in playerPrefabs)
            {
                if (player.name == prefab.name + "(Clone)")
                {
                    player.SetActive(false);
                }
            }
        }
    }

    private void DeActivateCharactersCanvas()
    {
        GameObject canvasGeneral = GameObject.Find("Canvas general(Clone)");
        if (canvasGeneral != null)
        {
            //canvasGeneral.SetActive(false);
            Destroy(canvasGeneral);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject canvasp1 = GameObject.Find("Canvas Player 1");
            if (canvasp1 != null)
            {
                //canvasp1.SetActive(false);
                Destroy(canvasp1);
            }
        }
        else
        {
            GameObject canvasp2 = GameObject.Find("Canvas Player 2");
            if (canvasp2 != null)
            {
                canvasp2.SetActive(false);
                Destroy(canvasp2);
            }
        }
    }
}
