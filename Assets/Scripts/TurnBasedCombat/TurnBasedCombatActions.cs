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
    public CharacterData charactersData;
    public List<GameObject> attackMenuList;

    private GameObject boss;
    private GameObject currentPlayerGameObject;

    float playerDamageMult, bossDefenseMult;

    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        boss = tbc.boss;
        playerDamageMult = tbc.playerAttackMultiplier;
        bossDefenseMult = tbc.BossDefenseMultiplier;
    }

    public GameObject SetCorrespondingActionsMenu(List<PlayerInNetwork> players)
    {
        foreach (PlayerInNetwork player in players)
        {
            if (player.IsLocal)
            {
                currentPlayerGameObject = player.tagObject as GameObject;
                return GetCorrespondingActionsMenu(currentPlayerGameObject.name);
            }
        }
        return null;
    }

    public GameObject GetCorrespondingActionsMenu(string charactersName)
    {
        string charactersOriginalName = charactersName.Replace("(Clone)", "");

        foreach (GameObject attackMenu in attackMenuList)
        {
            bool charactersAttackMenu = charactersData.characters.Exists(data => data.Name == charactersOriginalName && data.AttackMenu == attackMenu.name);
            if (charactersAttackMenu)
            {
                currentPlayerGameObject.GetComponent<MenuControllerCBT>().GetButtons(attackMenu.transform.GetChild(0).gameObject, attackMenu.transform.GetChild(1).gameObject, attackMenu.transform.GetChild(2).gameObject);
                return attackMenu;
            }
        }
        return null;
    }

    // Attack function. It takes the target and damage as parameters.
    public void Attack(GameObject target, int damage){
        if (target == boss){ // If target is boss, use a PunRPC to syncronize boss current health for all clients.
            PhotonView photonView = boss.GetComponent<PhotonView>();
            photonView.RPC("TakeDamage", RpcTarget.All, (int)((damage*playerDamageMult)/bossDefenseMult));
        }
        else{
            PhotonView photonView = target.GetComponent<PhotonView>();       
        }
    }

    // Attack function. It takes the target and damage as parameters.
    public void CharacterAttack(GameObject character)
    {
        GameObject characterGameObject = GameObject.Find(character.name + "(Clone)");
        characterGameObject.GetComponent<AttackInitializer>().StartAttackAnimation();
    }
}
