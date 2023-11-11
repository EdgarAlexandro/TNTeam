/* Function: Define the actions for the turn based combat actions menu
   Author: Daniel Degollado Rodríguez 
   Modification date: 10/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnBasedCombatActions : MonoBehaviour
{
    TurnBasedCombatManager tbc;

    GameObject boss;

    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        boss = tbc.boss;
    }
    // Attack function. It takes the target and damage as parameters.
    public void Attack(GameObject target, int damage){
        if (target == boss){ // If target is boss, use a PunRPC to syncronize boss current health for all clients.
            PhotonView photonView = boss.GetComponent<PhotonView>();
            photonView.RPC("TakeDamage", RpcTarget.All, damage);
        }
        else{
            PhotonView photonView = target.GetComponent<PhotonView>();       
        }
    }

    public void AttackBtn(){
        Attack(boss, 1);
        tbc.EndTurn();
    }
}
