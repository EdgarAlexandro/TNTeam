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
        float yInput = Input.GetAxis("Vertical");
        rig.velocity = new Vector2(yInput * moveSpeed, rig.velocity.x);
        switch (yInput)
        {
            case var _ when yInput < 0:
                UpdateAnimation(PlayerAnimation.walkDown);
                break;

            case var _ when yInput > 0:
                UpdateAnimation(PlayerAnimation.walkUp);
                break;

            default:
                animatorController.SetBool("isWalkingDown", false);
                animatorController.SetBool("isWalkingUp", false);
                break;
        }
    }

    private void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(xInput * moveSpeed, rig.velocity.y);
        if(xInput != 0)
        {
            UpdateAnimation(PlayerAnimation.walkSides);
            switch (xInput)
            {
                case var _ when xInput < 0:
                    transform.localScale = new Vector3(0.15f, 0.15f);
                    break;

                case var _ when xInput > 0:
                    transform.localScale = new Vector3(-0.15f, 0.15f);
                    break;
            }
        }
        else
        {
            animatorController.SetBool("isWalkingSides", false);
        }

    }
    public enum PlayerAnimation
    {
        walkDown, walkUp, walkSides
    }
    void UpdateAnimation(PlayerAnimation nameAnimation)
    {
        switch (nameAnimation)
        {
            case PlayerAnimation.walkDown:
                animatorController.SetBool("isWalkingDown", true);
                break;
            case PlayerAnimation.walkUp:
                animatorController.SetBool("isWalkingUp", true);
                break;
            case PlayerAnimation.walkSides:
                animatorController.SetBool("isWalkingSides", true);
                break;
        }
    }
}