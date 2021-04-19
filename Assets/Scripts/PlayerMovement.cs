using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    PlayerControls controls;

    Animator anim;

    public CharacterController controller;
    public Transform playerModel;

    public float speed = 12f;
    public float turnSpeed = 3f;

    Vector2 move;
    Vector2 rotate;

    bool isMoving;

    private void Awake()
    {
        //initialize player controls
        controls = new PlayerControls();

        controls.Gameplay.Attack.performed += ctx => Grow(); //change to attack later

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>(); //get input from left stick for movement
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); //get input from right stick for rotation
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        //get components needed
        anim = GetComponent<Animator>();
    }

    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    private void Update()
    {
        float x = move.x; //store move value into variables
        float y = move.y;
        

        Vector3 m = transform.right * x + transform.forward * y; //use move value to create the (m)ovement

        if (x != 0 && y != 0) //check to see if the player is actively inputting the move command
        {
            isMoving = true;
            anim.SetFloat("BlendX", x % 1);
            anim.SetFloat("BlendY", y % 1);
        } else
        {
            isMoving = false;
            anim.SetFloat("BlendX", 0);
            anim.SetFloat("BlendY", 0);
        }

        if (isMoving) //if the movement input is being done, then the player will move (done to allow the player to aim freely when not moving)
        {
            controller.Move(m * speed * Time.deltaTime);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(m), 0.15F);
            Debug.Log("I'm moving!");
        } else
        {
            Debug.Log("Not moving!");
        }

        //Vector2 r = new Vector2(0, rotate.x) * turnSpeed; //only the x is needed
        //playerModel.Rotate(r); //rotate the player's body specifically, based off turn speed
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();    
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
