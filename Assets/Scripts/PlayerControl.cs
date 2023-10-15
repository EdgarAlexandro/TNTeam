//
//PlayerControl.cs
//Script para controlar el movimiento y animaciones del personaje
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public bool isAttacking = false;
    private bool isDefending = false;

    [Header("Components")]
    public Rigidbody2D rig;
    public VectorValue startingPosition;
    Animator animatorController;
    public Player photonPlayer;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        animatorController = GetComponent<Animator>();
        //transform.position = startingPosition.initialValue;
    }

    void Update()
    {
        if (photonView.IsMine) // Verifica si es el jugador local
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            // Actualiza la velocidad del Rigidbody2D
            rig.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
            if (isAttacking)
            {
                rig.velocity = new Vector2(0, 0);
            }
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
            if (!isAttacking && !isDefending)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isAttacking = true;
                    animatorController.SetBool("isAttacking", true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isDefending = true;
                animatorController.SetBool("isDefending", true);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                isDefending = false;
                animatorController.SetBool("isDefending", false);
            }
        }
    }

    [PunRPC]
    public void Init(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        SpawnController.instance.players[id - 1] = this;

        if (!photonView.IsMine)
            rig.isKinematic = true;
    }

    void EndAttackAnimation()
    {
        animatorController.SetBool("isAttacking", false);
        isAttacking = false;
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
