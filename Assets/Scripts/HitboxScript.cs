using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    Quaternion playerRotation;
    Vector3 playerM;

    public Transform hitboxOrigin;
    public GameObject hitbox;

    bool isActive;

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

    public void Attack()
    {
        if (!isActive)
        {
            StartCoroutine(attackCoroutine());
        }
    }

    IEnumerator attackCoroutine()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        hitbox.SetActive(true);

        isActive = true;

        playerMovement.setMobility(false);

        yield return new WaitForSeconds(0.1f);

        hitbox.SetActive(false);

        isActive = false;

        yield return new WaitForSeconds(0.1f);

        playerMovement.setMobility(true);
    }
}
