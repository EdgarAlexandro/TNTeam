using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private GameObject bossObject;
    private Vector3 bossPosition;
    private float force = 15.0f;
    private bool hasCollided = false;

    private TurnBasedCombatManager tbc;

    // Start is called before the first frame update
    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bossObject = GameObject.Find("La Llorona");
        bossPosition = bossObject.transform.position;
    }

    void FixedUpdate()
    {
        if (!hasCollided)
        {
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
            hasCollided = true;
            rigidBody.velocity = Vector2.zero;
            animator.SetBool("Explosion", true);
            StartCoroutine(AlternateColors(bossObject));
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

    // Coroutine to alternate colors when players take damage
    public IEnumerator AlternateColors(GameObject boss)
    {
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        boss.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
    }

    public void ChangeTransparency()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0.4f);
    }
}
