using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private float range;
    public Transform target;
    private bool targetCollision = false;
    public float speed = 2.0f;
    private float minDistance = 5.0f;
    private float thrust = 2.0f;
    private Animator animatorController;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animatorController = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null)
        {
            UpdateAnimation(Vector3.zero);
            return;
        }
        range = Vector2.Distance(transform.position, target.position);
        if (range < minDistance)
        {
            if (!targetCollision)
            {
                Vector3 moveDirection = (target.position - transform.position).normalized;

                UpdateAnimation(moveDirection);

                transform.LookAt(target.position);
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            UpdateAnimation(Vector3.zero);
        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 triggerPosition = transform.position;
            Vector3 contactPoint = triggerPosition;
            //Vector3 contactPoint = other.contacts[0].point;
            Vector3 center = other.gameObject.GetComponent<Collider2D>().bounds.center;

            targetCollision = true;

            bool right = contactPoint.x > center.x;
            bool left = contactPoint.x < center.x;
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            Vector2 thrustDirection = Vector2.zero;

            if (right) thrustDirection += Vector2.right;
            if (left) thrustDirection += Vector2.left;
            if (top) thrustDirection += Vector2.up;
            if (bottom) thrustDirection += Vector2.down;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(thrustDirection.normalized * thrust, ForceMode2D.Impulse);

            Invoke("FalseCollision", 0.25f);
        }
    }

    void FalseCollision()
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

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

    public IEnumerator Freeze(){
        speed = 0.0f;
        Debug.Log("AI Frozen");
        yield return new WaitForSeconds(6);
        speed = 2.0f;
        Debug.Log("AI Moving");
    }
}
