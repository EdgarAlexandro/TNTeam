using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    private float pressed;
    private float released;
    bool keyPressed;
    // Update is called once per frame
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
            Debug.Log("Time between press and release: " + timeDifference + " seconds");
            keyPressed = false;
        }
    }
}
