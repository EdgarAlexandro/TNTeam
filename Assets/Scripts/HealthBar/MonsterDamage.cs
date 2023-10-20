using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterDamage : MonoBehaviourPunCallbacks
{
    public int damage;
    public UIController playerUI;
    public bool isHittingShield = false;
    public float knockback;
    private IEnumerator coroutine;

    public void Update()
    {
        if (GetComponent<EnemyAi>().currentTarget != null)
        {
            playerUI = GetComponent<EnemyAi>().currentTarget.GetComponent<UIController>();
        }
    }

    private IEnumerator applyDamage(string tarjetTakingDamage)
    {
        yield return new WaitForSeconds(0.0f);
        if (!isHittingShield)
        {
            playerUI.TakeDamage(damage, tarjetTakingDamage);

        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            //Invoke("applyDamage", 0.0f);
            coroutine = applyDamage(other.gameObject.name);
            StartCoroutine(coroutine);
        }
    }
}
