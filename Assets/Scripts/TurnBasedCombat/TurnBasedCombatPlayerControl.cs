/* Function: Player control of the turn based combat character
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCombatPlayerControl : MonoBehaviourPunCallbacks
{
    Animator animatorController;
    public bool canBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<Animator>();
    }

    void Update(){
        if (canBlock && photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
        {
            // Use shield to avoid taking damage from boss attack
            StartCoroutine("ShieldAction");
        }
    }

    IEnumerator ShieldAction()
    {
        animatorController.SetBool("Defensa", true);
        yield return new WaitForSeconds(0.5f);                  
        animatorController.SetBool("Defensa", false);
        canBlock = false;
    }
}
