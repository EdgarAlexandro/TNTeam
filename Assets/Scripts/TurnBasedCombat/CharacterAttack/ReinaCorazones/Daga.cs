using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Daga : MonoBehaviourPunCallbacks
{
    private GameObject bossObject;
    private Vector3 bossPosition;
    public float force = 1.0f;
    private Rigidbody2D rigidBody;
    private TurnBasedCombatManager tbc;
    

    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
        bossObject = GameObject.Find("La Llorona");  // Get Boss GameObject
        bossPosition = bossObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsBoss();
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
            StartCoroutine(AlternateColors(bossObject));
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

    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(GameObject boss)
    {
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
    }
}
