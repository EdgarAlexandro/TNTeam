/* Function: controls the behaviour of the player´s shield
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Tells the enemy it is hitting a shield (does no damage)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            MonsterDamage monsterDamage = other.gameObject.GetComponent<MonsterDamage>();
            if(monsterDamage != null)
            {
                monsterDamage.isHittingShield = true;
            }
            /*else
            {
                BossAttack bossAttack = other.gameObject.GetComponent<BossAttack>();
                if(bossAttack != null)
                {
                    Debug.Log("HitSHield");
                    bossAttack.isHittingShield = true;
                }
            }*/

        }
    }
    // Tells the enemy it is not hitting a shield (does damage)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            MonsterDamage monsterDamage = other.gameObject.GetComponent<MonsterDamage>();
            if (monsterDamage != null)
            {
                monsterDamage.isHittingShield = false;
            }
            /*else
            {
                BossAttack bossAttack = other.gameObject.GetComponent<BossAttack>();
                if (bossAttack != null)
                {
                    bossAttack.isHittingShield = false;
                }
            }*/
        }
    }
}
