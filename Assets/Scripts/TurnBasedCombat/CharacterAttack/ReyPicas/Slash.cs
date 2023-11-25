using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slash : MonoBehaviourPunCallbacks
{
    private Animator animator;
    private GameObject bossObject;
    private Vector3 bossPosition;

    private TurnBasedCombatManager tbc;

    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        animator = GetComponent<Animator>();
        bossObject = GameObject.Find("La Llorona");
        bossPosition = bossObject.transform.position;
        TeleportToBoss();
    }

    public void AttackBoss()
    {
        if (photonView.IsMine)
        {
            bossObject.GetComponent<BossHealth>().TakeDamage(5);
        }
        StartCoroutine(AlternateColors(bossObject));
    }

    public void DestroyObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            tbc.EndTurn();
        }
    }

    public void TeleportToBoss()
    {
        transform.position = bossPosition;
        animator.SetBool("Slash", true);
    }

    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(GameObject boss)
    {
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        //photonView.RPC("ChangeColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        //photonView.RPC("ReturnColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
    }
}
