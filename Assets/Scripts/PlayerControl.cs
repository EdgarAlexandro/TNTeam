//
//PlayerControl.cs
//Script para controlar el movimiento y animaciones del personaje
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rig;
    Animator animatorController;

    void Start()
    {
        animatorController = GetComponent<Animator>();
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        // Actualiza la velocidad del Rigidbody2D
        rig.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);

        // Calcula la dirección predominante (horizontal o vertical)
        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            // Movimiento horizontal
            if (xInput > 0)
            {
                UpdateAnimation(PlayerAnimation.walkRight);
            }
            else if (xInput < 0)
            {
                UpdateAnimation(PlayerAnimation.walkLeft);
            }
        }
        else
        {
            // Movimiento vertical
            if (yInput > 0)
            {
                UpdateAnimation(PlayerAnimation.walkUp);
            }
            else if (yInput < 0)
            {
                UpdateAnimation(PlayerAnimation.walkDown);
            }
            else
            {
                animatorController.SetBool("isWalkingDown", false);
                animatorController.SetBool("isWalkingUp", false);
                animatorController.SetBool("isWalkingRight", false);
                animatorController.SetBool("isWalkingLeft", false);
            }
        }
    }

    public enum PlayerAnimation
    {
        walkDown, walkUp, walkRight, walkLeft
    }

    void UpdateAnimation(PlayerAnimation nameAnimation)
    {
        animatorController.SetBool("isWalkingDown", false);
        animatorController.SetBool("isWalkingUp", false);
        animatorController.SetBool("isWalkingRight", false);
        animatorController.SetBool("isWalkingLeft", false);
        switch (nameAnimation)
        {
            case PlayerAnimation.walkDown:
                animatorController.SetBool("isWalkingDown", true);
                break;
            case PlayerAnimation.walkUp:
                animatorController.SetBool("isWalkingUp", true);
                break;
            case PlayerAnimation.walkRight:
                animatorController.SetBool("isWalkingRight", true);
                break;
            case PlayerAnimation.walkLeft:
                animatorController.SetBool("isWalkingLeft", true);
                break;
        }
    }
}
