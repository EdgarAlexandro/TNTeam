using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QueenOfClubsFreezeField : MonoBehaviourPunCallbacks
{
    public GameObject fieldPrefab;
    // Update is called once per frame field.name
    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ActivateFreezeField());
        }
        
    }

    IEnumerator ActivateFreezeField()
    {
        GetComponent<PlayerControl>().isAttacking = true;
        GameObject fieldInstance = PhotonNetwork.Instantiate(fieldPrefab.name, transform.position, Quaternion.identity);
        CircleCollider2D fieldCollider = fieldInstance.GetComponent<CircleCollider2D>();
        yield return new WaitForSeconds(3);
        Debug.Log("End Attack;");
        PhotonNetwork.Destroy(fieldInstance);
        GetComponent<PlayerControl>().isAttacking = false;
    }
}
