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

    float comboAttackTimer;

    int chargeLevel = 0;

    int comboLevel = 0;
    int prevCombo;

    Vector2 move;
    Vector2 rotate;

    Vector3 m;

    Quaternion playerRotation;

    bool isMoving;
    bool isMobile;
    bool attackHeld = false;
    bool attackInitiated = false;
    bool firstAttack = true;

    private void Awake()
    {
        //initialize player controls
        controls = new PlayerControls();

        controls.Gameplay.Attack.performed += ctx => attackHeld = true; //flag attack as held
        controls.Gameplay.Attack.canceled += ctx => attackHeld = false; //flag attack as no longer held

        controls.Gameplay.Attack.performed += ctx => attackInitiated = true; //flag attack as initiated

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

        Debug.Log("Attacking with a " + comboLevel + " Combo!");

        if (comboLevel > 1)
        {
            hitboxScript.setDamage(30);
        } else
        {
            hitboxScript.setDamage(25);
        }

        

        anim.SetTrigger("BasicAttack"); //attack animation

        hitboxScript.Attack(false); //generates the hitbox

        attackInitiated = false;
    }

    void ChargedStrike()
    {
        HitboxScript hitboxScript = FindObjectOfType<HitboxScript>();

        if (chargeLevel == 1)
        {
            hitboxScript.setDamage(30);
        } else if (chargeLevel == 2)
        {
            hitboxScript.setDamage(50);
        } else if (chargeLevel == 3)
        {
            hitboxScript.setDamage(100);
        }
        

        anim.SetTrigger("SpinAttack"); //attack animation

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

        if (isMoving && isMobile) //if the movement input is being done, then the player will move; also checks if player is mobile
        {
            controller.Move(m * speed * Time.deltaTime);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(m), 0.15F);

            playerRotation = playerModel.rotation; //store the player's rotation so it can be read by other scripts

            Debug.Log("I'm moving!");
        } else
        {
            Debug.Log("Not moving!");
        }

        updateChargeTimer();

        updateComboTimer();
    }

    public void Attack(bool held)
    {
        Debug.Log("attacking");

        if (held == true) //sees if button is being held down
        {
            if (chargeAttackTimer > 0.26)
            {
                anim.SetTrigger("Charging");
                isMobile = false;
            }
        }

        if (held == false) //only run on release of the button
        {

            if (chargeAttackTimer < 0.26) //check to see if the attack is charged
            {
                if (comboAttackTimer <= 0.35)
                {
                    comboLevel++;
                    Debug.Log("Combo is " + comboLevel);
                }
                if (comboLevel > 3)
                {
                    comboLevel = 0;
                }

                BasicStrike();

                Debug.Log("Not Charged!");
            }
            else
            {
                if (chargeAttackTimer < 0.33)
                {
                    chargeLevel = 1;
                } else if (chargeAttackTimer > 0.33 && chargeAttackTimer < 0.66)
                {
                    chargeLevel = 2;
                } else if (chargeAttackTimer > 0.66)
                {
                    chargeLevel = 3;
                }

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

    

    void updateChargeTimer() //update timer for holding down your charged attack
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

    void updateComboTimer()
    {

        if (comboLevel > prevCombo)
        {
            comboAttackTimer -= 0.35f;
        }

        if (comboLevel > 0)
        {
            comboAttackTimer += Time.deltaTime;
        } else
        {
            comboAttackTimer = 0;
        }

        if (comboAttackTimer >= 0.35f)
        {
            comboAttackTimer = 0;
            comboLevel = 0;
        }

        prevCombo = comboLevel;
    }

    public float returnChargeTimer()
    {
        return chargeAttackTimer;
    }
}
