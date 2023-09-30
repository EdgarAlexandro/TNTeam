using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth playerHealth;
    public bool isHittingShield = false;
    public float knockback;
    private IEnumerator coroutine;

    private IEnumerator applyDamage()
    {
        yield return new WaitForSeconds(0.0f);
        if (!isHittingShield)
        {
            playerHealth.TakeDamage(damage);

        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Invoke("applyDamage", 0.0f);
            coroutine = applyDamage();
            StartCoroutine(coroutine);
        }
    }
}
