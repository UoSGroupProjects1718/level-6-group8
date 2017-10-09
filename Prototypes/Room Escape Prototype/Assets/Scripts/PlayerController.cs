using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Variables
    public float speed = 8.0F;
    public float runSpeed = 12.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    // Static
    public static bool useGravity;

    void Start()
    {
        useGravity = true;
    }


    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded || !useGravity)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            //Multiply it by speed.
            if (Input.GetKey(KeyCode.LeftShift))
                moveDirection *= runSpeed;
            else
                moveDirection *= speed;

            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }

        //Applying gravity to the controller
        if (useGravity)
            moveDirection.y -= gravity * Time.deltaTime;

        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
    }
}
