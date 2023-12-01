/* Function: Enemies AI for their movement
   Author: Daniel Degollado Rodr�guez
   Modification date: 27/10/2023 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAi : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;
    public GameObject[] targets;
    public GameObject currentTarget;
    private bool targetCollision = false;
    public float speed = 2.0f;
    private float minDistance = 5.0f;
    private float thrust = 2.0f;
    public int health;
    public int maxHealth;
    public bool freezed = false;
    private Animator animatorController;
    private SpriteRenderer sprite;
    private MusicSFXManager musicSFXManager;
    private Rigidbody2D rigidBod;
    NavMeshAgent agent;
    public GameObject orbePrefab = null;

    // Remote Procedure that destroys the enemy for all players
    [PunRPC]
    public void DestroyEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void Start()
    {
        rigidBod = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
        health = maxHealth;
        musicSFXManager = MusicSFXManager.Instance;
        sprite = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        // Set enemy's target if a player enters a determined range
        if ((targets.Length >= 1) && (Vector2.Distance(transform.position, targets[0].transform.position) < minDistance)) currentTarget = targets[0];
        else if (!PhotonNetwork.OfflineMode && targets.Length >= 2 && Vector2.Distance(transform.position, targets[1].transform.position) < minDistance) currentTarget = targets[1];
        else currentTarget = null;

        if (currentTarget != null)
        {

            // Sets the enemy destination to the target (Navmesh).
            if (!targetCollision && !freezed){
                if(currentTarget.gameObject.GetComponent<UIController>().isDead == false){
                    rigidBod.constraints = RigidbodyConstraints2D.None;
                    Vector2 targetPosition = currentTarget.transform.position;

                    agent.SetDestination(targetPosition);

                    Vector2 moveDirection = agent.desiredVelocity.normalized;

                    UpdateAnimation(moveDirection);

                    transform.LookAt(targetPosition);
                    transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                }
                else
                {
                    currentTarget = null;
                }
            }


        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            // Set agents destination to its own to stop movement (Navmesh)
            agent.SetDestination(transform.position);
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Mordisco);
            Vector2 triggerPosition = agent.desiredVelocity;
            //Vector3 triggerPosition = transform.position;
            Vector3 contactPoint = triggerPosition;
            Vector3 center = other.gameObject.GetComponent<Collider2D>().bounds.center;

            targetCollision = true;
            // Enemies go back a little after attacking a player
            bool right = contactPoint.x > center.x;
            bool left = contactPoint.x < center.x;
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            Vector2 thrustDirection = Vector2.zero;

            if (right) thrustDirection += Vector2.right;
            if (left) thrustDirection += Vector2.left;
            if (top) thrustDirection += Vector2.up;
            if (bottom) thrustDirection += Vector2.down;

            //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(thrustDirection.normalized * thrust, ForceMode2D.Impulse);

            //rigidBod.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke("FalseCollision", 0.25f);
        }
    }
    // Set enemy collistion state to false
    void FalseCollision()
    {
        targetCollision = false;
        //GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //rigidBod.constraints = RigidbodyConstraints2D.None;
    }
    // Updates enemy's animation
    void UpdateAnimation(Vector3 moveDirection)
    {
        animatorController.SetBool("isWalkingDown", false);
        animatorController.SetBool("isWalkingUp", false);
        animatorController.SetBool("isWalkingRight", false);
        animatorController.SetBool("isWalkingLeft", false);
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            if (moveDirection.x > 0) animatorController.SetBool("isWalkingRight", true);
            else if (moveDirection.x < 0) animatorController.SetBool("isWalkingLeft", true);
        }
        else
        {
            if (moveDirection.y > 0) animatorController.SetBool("isWalkingUp", true);
            else if (moveDirection.y < 0) animatorController.SetBool("isWalkingDown", true);
        }
    }
    // Enemies take damage. Takes the damage amount, the direction of the knockack and it's amount as parameters.
    public void OnHit(int damageAmount, Vector2 knockbackDirection, float knockbackAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            photonView.RPC("DestroyEnemy", RpcTarget.All);
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Muerte_Nahual);
            PhotonNetwork.Instantiate(orbePrefab.name, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackAmount, ForceMode2D.Impulse);
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(StopMovementAfterKnockback());

        }
    }
    // Enemy stops after getting a knockback from a player's attack.
    private IEnumerator StopMovementAfterKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        sprite.color = new Color(255, 255, 255, 255);
    }
    // Freeze enemy. 
    public IEnumerator Freeze(){
        freezed = true; 
        //UpdateAnimation(Vector3.zero);
        agent.SetDestination(transform.position);
        rigidBod.constraints = RigidbodyConstraints2D.FreezeAll;
        //speed = 0.0f;
        animatorController.enabled = false;
        Debug.Log("AI Frozen");
        yield return new WaitForSeconds(6);
        freezed = false;
        animatorController.enabled = true;
    }
}