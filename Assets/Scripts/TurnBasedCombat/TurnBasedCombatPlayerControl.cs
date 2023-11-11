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
    public bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<Animator>();
    }

    void Update(){
        if (isActive){
            // If the player owns this character
            if (photonView.IsMine){
                // Use shield to avoid taking damage from boss attack
                if (Input.GetKeyDown(KeyCode.Q)){
                    animatorController.SetBool("isDefending", true);
                }
                if (Input.GetKeyUp(KeyCode.Q)){
                    animatorController.SetBool("isDefending", false);
                }
            }
        }
    }

    /*IEnumerator ActiveShield()
    {
        yield return 
    }*/
}
