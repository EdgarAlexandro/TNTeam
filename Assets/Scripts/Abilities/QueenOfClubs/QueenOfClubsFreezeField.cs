using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenOfClubsFreezeField : MonoBehaviour
{
    public CircleCollider2D field;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(ActivateFreezeField());
        }
    }

    IEnumerator ActivateFreezeField(){
        GetComponent<PlayerControl>().isAttacking = true;
        CircleCollider2D AtrInstance = Instantiate(field, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        Debug.Log("End Attack;");
        Destroy(AtrInstance.gameObject);
        GetComponent<PlayerControl>().isAttacking = false;
    }
}
