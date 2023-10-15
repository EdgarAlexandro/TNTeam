using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    private float pressed;
    private float released;
    Animator animatorController;
    public bool keyPressed;
    public int chargeDmg = 1;

    public bool isChargeAttacking = false;
    //bool isAttacking = false;
    // Update is called once per frame

    void Start()
    {
        animatorController = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !keyPressed)
        {
            pressed = Time.time;
            keyPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && keyPressed)
        {
            released = Time.time;
            float timeDifference = released - pressed;
            keyPressed = false;
            Debug.Log("Time between press and release: " + timeDifference + " seconds");
            GetComponent<PlayerControl>().isAttacking = true;
            animatorController.SetBool("isAttacking", true);
            //isChargeAttacking = true;

            
            if (timeDifference > 6f)
            {
                isChargeAttacking = true;
                chargeDmg = 6;
                //hacer dano 100
            }
            else if (timeDifference > 4f)
            {
                isChargeAttacking = true;
                chargeDmg = 4;
                //hacer dano 150
            }
            else if (timeDifference > 2f)
            {
                isChargeAttacking = true;
                chargeDmg = 2;
                //hacer dano 200
            }
        }
    }
}
