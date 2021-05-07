using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    Quaternion playerRotation;
    Vector3 playerM;

    public Transform hitboxOrigin;
    public GameObject basicHitbox;
    public GameObject chargedHitbox;

    [SerializeField] AudioClip stabSFX;
    [SerializeField] AudioClip swooshSFX;

    bool isActive;

    int attackDamage = 0;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerRotation = setPlayerRotation();
        playerM = setPlayerM();

        if (getMobility() == true)
        {
            hitboxOrigin.rotation = Quaternion.Slerp(playerRotation, Quaternion.LookRotation(playerM), 0.15F); //same values applied to the player, now applied to the hitbox at an anchor point within the player
        }
    }

    bool getMobility()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        return playerMovement.returnMobility();
    }

    Quaternion setPlayerRotation() //get players current rotation value
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        Quaternion r = playerMovement.returnPlayerRotation();

        return r;
    }

    Vector3 setPlayerM() //get players current movement direction
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        Vector3 m = playerMovement.returnPlayerM();

        return m;
    }

    public int returnDamage()
    {
        return attackDamage;
    }

    public void setDamage(int damage)
    {
        attackDamage = damage;
    }

    public void Attack(bool charged)
    {
        if (!isActive)
        {
            StartCoroutine(attackCoroutine(charged));
        }
    }

    IEnumerator attackCoroutine(bool charged)
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        float delay = 0.1f;

        if (charged)
        {
            delay *= 4;
        }

        if (charged)
        {
            chargedHitbox.SetActive(true);
            AudioHelper.PlayClip2D(swooshSFX, 0.7f);
        } else
        {
            basicHitbox.SetActive(true);
            AudioHelper.PlayClip2D(stabSFX, 0.7f);
        }
        

        isActive = true;

        playerMovement.setMobility(false);

        yield return new WaitForSeconds(0.1f);

        if (charged)
        {
            chargedHitbox.SetActive(false);
        }
        else
        {
            basicHitbox.SetActive(false);
        }

        isActive = false;

        yield return new WaitForSeconds(0.1f);

        playerMovement.setMobility(true);
    }
}
