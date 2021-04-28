using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{

    PlayerControls controls;

    Animator anim;

    public CharacterController controller;
    public Transform playerModel;

    public float speed = 12f;
    public float turnSpeed = 3f;

    float chargeAttackTimer;

    Vector2 move;
    Vector2 rotate;

    Vector3 m;

    Quaternion playerRotation;

    bool isMoving;
    bool isMobile;
    bool attackHeld = false;
    bool attackInitiated = false;

    private void Awake()
    {
        //initialize player controls
        controls = new PlayerControls();

        controls.Gameplay.Attack.performed += ctx => attackHeld = true; //flag attack as held
        controls.Gameplay.Attack.canceled += ctx => attackHeld = false; //flag attack as no longer held

        controls.Gameplay.Attack.performed += ctx => attackInitiated = true; //flag attack as initiated
        //controls.Gameplay.Attack.canceled += ctx => attackInitiated = false; //change to attack later

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>(); //get input from left stick for movement
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); //get input from right stick for rotation
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        //get components needed
        anim = GetComponent<Animator>();

        //make sure player is mobile
        isMobile = true;

        //set timer to zero
        chargeAttackTimer = 0;
    }

    void BasicStrike()
    {
        HitboxScript hitboxScript = FindObjectOfType<HitboxScript>();

        anim.SetTrigger("BasicAttack"); //attack animation

        hitboxScript.Attack(false); //generates the hitbox

        attackInitiated = false;
    }

    void ChargedStrike()
    {
        HitboxScript hitboxScript = FindObjectOfType<HitboxScript>();

        anim.SetTrigger("Charging"); //attack animation

        hitboxScript.Attack(true); //generates the hitbox

        attackInitiated = false;
    }

    private void Update()
    {
        float x = move.x; //store move value into variables
        float y = move.y;

        m = transform.right * x + transform.forward * y; //use move value to create the (m)ovement

        if (attackInitiated)
        {
            Attack(attackHeld);
        }

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

        if (isMoving && isMobile) //if the movement input is being done, then the player will move (done to allow the player to aim freely when not moving); also checks if player is mobile
        {
            controller.Move(m * speed * Time.deltaTime);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(m), 0.15F);

            playerRotation = playerModel.rotation; //store the player's rotation so it can be read by other scripts

            Debug.Log("I'm moving!");
        } else
        {
            Debug.Log("Not moving!");
        }

        chargeTimer();

        //Vector2 r = new Vector2(0, rotate.x) * turnSpeed; //only the x is needed
        //playerModel.Rotate(r); //rotate the player's body specifically, based off turn speed
    }

    public void Attack(bool held)
    {
        Debug.Log("attacking");

        if (held == true) //sees if button is being held down
        {
            if (chargeAttackTimer > 0.2)
            {
                anim.SetTrigger("Charging");
            }
            Debug.Log("piss");
        }

        if (held == false) //only run on release of the button
        {

            if (chargeAttackTimer < 1) //check to see if the attack is charged
            {
                BasicStrike();
                Debug.Log("Not Charged!");
            }
            else
            {
                ChargedStrike();
                Debug.Log("Charged!");
            }

            attackHeld = false;
        }

        //attackInitiated = false;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();    
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public Quaternion returnPlayerRotation()
    {
        return playerRotation;
    }

    public Vector3 returnPlayerM()
    {
        return m;
    }

    public void setMobility(bool b)
    {
        isMobile = b;
    }

    public bool returnMobility()
    {
        return isMobile;
    }

    

    void chargeTimer() //update timer for holding down your charged attack
    {
        if (attackHeld == true)
        {
            chargeAttackTimer += Time.deltaTime;
        }
        else
        {
            chargeAttackTimer = 0;
        }
    }
}
