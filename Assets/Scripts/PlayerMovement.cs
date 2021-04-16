using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    PlayerControls controls;

    public CharacterController controller;
    public Transform playerBody;

    public float speed = 12f;
    public float turnSpeed = 3f;

    Vector2 move;
    Vector2 rotate;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Attack.performed += ctx => Grow(); //change to attack later

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>(); //get input from left stick for movement
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); //get input from right stick for rotation
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    private void Update()
    {
        //Vector3 m = new Vector3(move.x, 0, move.y);

        Vector3 m = transform.right * move.x + transform.forward * move.y;

        controller.Move(m * speed * Time.deltaTime);

        Vector2 r = new Vector2(0, rotate.x) * turnSpeed; //only the x is needed
        playerBody.Rotate(r); //rotate the player's body specifically, based off turn speed
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
