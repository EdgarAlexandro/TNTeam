/* Function: controls the animations, shield, attack and movement of the player/character
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id = 0;

    [Header("Info")]
    public float moveSpeed = 0.0f;
    public bool isAttacking = false;
    private bool isDefending = false;

    [Header("Components")]
    public Rigidbody2D rig = null;
    public VectorValue startingPosition = null;
    Animator animatorController = null;
    public Player photonPlayer = null;

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
        // If the player owns this character (multiplayer)
        if (photonView.IsMine)
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");
            rig.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);

            // Stop movement if attacking
            if (isAttacking)
            {
                rig.velocity = new Vector2(0, 0);
            }

            // Calculate the predominant direction (horizontal or vertical)
            if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
            {
                if (xInput > 0) UpdateAnimation(PlayerAnimation.walkRight);
                else if (xInput < 0) UpdateAnimation(PlayerAnimation.walkLeft);
            }
            else
            {
                if (yInput > 0) UpdateAnimation(PlayerAnimation.walkUp);
                else if (yInput < 0) UpdateAnimation(PlayerAnimation.walkDown);
                else
                {
                    animatorController.SetBool("isWalkingDown", false);
                    animatorController.SetBool("isWalkingUp", false);
                    animatorController.SetBool("isWalkingRight", false);
                    animatorController.SetBool("isWalkingLeft", false);
                }
            }

            //Attack
            if (!isAttacking && !isDefending)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isAttacking = true;
                    animatorController.SetBool("isAttacking", true);
                }
            }

            // Use shield (hold)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isDefending = true;
                animatorController.SetBool("isDefending", true);
            }

            // Stop using shield
            if (Input.GetKeyUp(KeyCode.Q))
            {
                isDefending = false;
                animatorController.SetBool("isDefending", false);
            }
        }
    }

    // Remote Procedure Call to initialize players
    [PunRPC]
    public void Init(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        SpawnController.instance.players[id - 1] = this;

        if (!photonView.IsMine)
            rig.isKinematic = true;
    }

    // Stop attack (used by animator)
    public void EndAttackAnimation()
    {
        animatorController.SetBool("isAttacking", false);
        isAttacking = false;
    }

    // List of possible movement animations
    public enum PlayerAnimation
    {
        walkDown, walkUp, walkRight, walkLeft
    }

    // Changes the movement animation
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
