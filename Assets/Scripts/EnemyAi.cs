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
    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        range = Vector2.Distance(transform.position, target.position);
 ;
        if (range < minDistance)
        {
            if (!targetCollision)
            {
                transform.LookAt(target.position);

                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        transform.rotation = Quaternion.identity;


    }
private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          Vector3 contactPoint = collision.contacts[0].point;
           Vector3 center = collision.collider.bounds.center;

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

}
