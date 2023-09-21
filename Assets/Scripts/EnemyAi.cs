using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private float range;
    public Transform target;
    private bool targetCollision = false;
    private float speed = 2.0f;
    private float minDistance = 5.0f;
    private float thrust = 2.0f;
    private Animator animatorController;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animatorController = GetComponent<Animator>();
        Collider myCollider = GetComponent<Collider>();
    }

    void Update()
    {
        range = Vector2.Distance(transform.position, target.position);
        if (range < minDistance)
        {
            if (!targetCollision)
            {
                Vector3 moveDirection = (target.position - transform.position).normalized;
                float horizontal = moveDirection.x;
                float vertical = moveDirection.y;
                if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                {
                    if (horizontal > 0)
                    {
                        UpdateAnimation(EnemyAnimation.idle);
                        UpdateAnimation(EnemyAnimation.walkRight);
                    }
                    else if (horizontal < 0)
                    {
                        UpdateAnimation(EnemyAnimation.idle);
                        UpdateAnimation(EnemyAnimation.walkLeft);
                    }
                }
                else
                {
                    if (vertical > 0)
                    {
                        UpdateAnimation(EnemyAnimation.idle);
                        UpdateAnimation(EnemyAnimation.walkUp);
                    }
                    else if (vertical < 0)
                    {
                        UpdateAnimation(EnemyAnimation.idle);
                        UpdateAnimation(EnemyAnimation.walkDown);
                    }
                }

                transform.LookAt(target.position);
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            UpdateAnimation(EnemyAnimation.idle);
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

            if (right) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            if (left) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            if (top) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            if (bottom) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            Invoke("FalseCollision", 0.25f);
        }
    }

    void FalseCollision()
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    public enum EnemyAnimation
    {
        idle, walkDown, walkUp, walkRight, walkLeft
    }
    void UpdateAnimation(EnemyAnimation nameAnimation)
    {
        switch (nameAnimation)
        {
            case EnemyAnimation.idle:
                animatorController.SetBool("isWalkingDown", false);
                animatorController.SetBool("isWalkingUp", false);
                animatorController.SetBool("isWalkingRight", false);
                animatorController.SetBool("isWalkingLeft", false);
                break;
            case EnemyAnimation.walkDown:
                animatorController.SetBool("isWalkingDown", true);
                break;
            case EnemyAnimation.walkUp:
                animatorController.SetBool("isWalkingUp", true);
                break;
            case EnemyAnimation.walkRight:
                animatorController.SetBool("isWalkingRight", true);
                break;
            case EnemyAnimation.walkLeft:
                animatorController.SetBool("isWalkingLeft", true);
                break;
        }
    }
}
