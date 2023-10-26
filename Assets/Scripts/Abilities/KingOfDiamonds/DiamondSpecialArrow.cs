using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSpecialArrow : MonoBehaviour
{
    public Rigidbody2D arrow;
    public float speed = 3;
    private Vector2 projDirection = Vector2.down;
    private MusicSFXManager musicSFXManager;


    // Start is called before the first frame update
    void Start()
    {
        projDirection = Vector2.down;
        musicSFXManager = MusicSFXManager.Instance;
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
            Rigidbody2D arrowInstance = Instantiate(arrow, transform.position, Quaternion.identity);
            arrowInstance.velocity = projDirection * speed;
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Arco);
        }
    }
}
