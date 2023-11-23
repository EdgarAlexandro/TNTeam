using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Thunder : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private GameObject bossObject;
    private GameObject attackSpawnPosition;
    private float speed = 25.0f;

    private TurnBasedCombatManager tbc;
    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackSpawnPosition = GameObject.Find("ReinaTrebolesAttackSpawn");
        bossObject = GameObject.Find("La Llorona");
    }

    // Update is called once per frame
    /*void Update()
    {
        MoveTowardsBoss();
    }*/

    void FixedUpdate()
    {
        MoveTowardsBoss();
    }

    public void MoveTowardsBoss()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, attackSpawnPosition.transform.position, speed * Time.deltaTime);
        rigidBody.MovePosition(newPosition);
        float distance = Vector3.Distance(transform.position, attackSpawnPosition.transform.position);
        if (distance < 11.06)
        {
            animator.SetBool("Atacando", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            if (photonView.IsMine)
            {
                other.GetComponent<BossHealth>().TakeDamage(5);
            }
            StartCoroutine(AlternateColors(other.gameObject.name));
        }
    }

    public void DestroyObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            tbc.EndTurn();
        }
    }

    [PunRPC]
    public void ChangeColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
    }
    // Remote procedure call to restore player's color
    [PunRPC]
    public void ReturnColor(string player)
    {
        GameObject playerGO = GameObject.Find(player);
        playerGO.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(string player)
    {
        photonView.RPC("ChangeColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
        photonView.RPC("ReturnColor", RpcTarget.All, player);
        yield return new WaitForSeconds(0.1f);
    }
}
