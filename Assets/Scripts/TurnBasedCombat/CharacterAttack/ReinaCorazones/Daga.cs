using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Daga : MonoBehaviourPunCallbacks
{
    private Vector3 bossPosition;
    private bool isMoving = false;
    public float force = 1.0f;
    private Rigidbody2D rigidBody;
    private TurnBasedCombatManager tbc;

    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
        GameObject bossObject = GameObject.Find("La Llorona");  // Get Boss GameObject
        bossPosition = bossObject.transform.position;
        photonView.RPC("SyncronizePosition", RpcTarget.All, bossPosition); // Call PunRPC so the targeted player's position is the same for all clients.
        photonView.RPC("SyncronizeAttackStatus", RpcTarget.All, true);
    }

    [PunRPC]
    public void SyncronizeAttackStatus(bool status)
    {
        isMoving = status;
    }

    //PunRPC to set the targeted player's position for all clients.
    [PunRPC]
    public void SyncronizePosition(Vector3 attackNewPosition)
    {
        bossPosition = attackNewPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {// If attack hasn't been destroyed call PunRPC so attack moves towards the targeted player for all clients.
            MoveTowardsBoss();
        }
    }

    public void MoveTowardsBoss()
    {
        rigidBody.gravityScale = 0.0f;
        rigidBody.AddForce((bossPosition - transform.position).normalized * force);
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
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
    }

    private void OnBecameInvisible()
    {
        // The object became invisible, destroy it.
        DestroyObject();
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
