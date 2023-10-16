/* Function: controls the behaviour of the player´s shield
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Tells the enemy it is hitting a shield (makes no damage)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<MonsterDamage>().isHittingShield = true;
        }
    }
    // Tells the enemy it is not hitting a shield (makes damage)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<MonsterDamage>().isHittingShield = false;
        }
    }
}
