using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenDaggerThrow : MonoBehaviour
{
    public Rigidbody2D dagger;
    public float speed = 3;
    private Vector2 projDirection = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        projDirection = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        

        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            // Movimiento horizontal
            if (xInput > 0)
            {
                projDirection = Vector2.right;
            }
            else if (xInput < 0)
            {
                projDirection = Vector2.left;
            }
        }
        else
        {
            // Movimiento vertical
            if (yInput > 0)
            {
                projDirection = Vector2.up;
            }
            else if (yInput < 0)
            {
                projDirection = Vector2.down;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(ThrowDagger());
        }
    }

    IEnumerator ThrowDagger(){ //async? void?
        Rigidbody2D daggerInstance = Instantiate(dagger, transform.position, Quaternion.identity);
        daggerInstance.velocity = projDirection * speed;
        yield return new WaitForSeconds (3);
        Destroy(daggerInstance.gameObject);
    }
}
