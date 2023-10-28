using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QueenOfClubsFreezeField : MonoBehaviourPunCallbacks
{
    private MusicSFXManager musicSFXManager;
    public CircleCollider2D field;
    public GameObject fieldPrefab;

    void Start()
    {
        musicSFXManager = MusicSFXManager.Instance;
    }
    
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
        if (musicSFXManager != null)
        {
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Campo_Fuerza);
        }
        GameObject fieldInstance = PhotonNetwork.Instantiate(fieldPrefab.name, transform.position, Quaternion.identity);
        CircleCollider2D fieldCollider = fieldInstance.GetComponent<CircleCollider2D>();
        yield return new WaitForSeconds(3);
        Debug.Log("End Attack;");
        PhotonNetwork.Destroy(fieldInstance);
        GetComponent<PlayerControl>().isAttacking = false;
    }
}
