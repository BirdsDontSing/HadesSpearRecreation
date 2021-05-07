using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{

    float currentChargeTimer;
    bool firstFlash = false;
    bool secondFlash = false;
    bool chargeStarted = false;

    private GameObject sfxClip;

    [SerializeField] Material activatedMaterial;
    [SerializeField] Material baseMaterial;
    [SerializeField] GameObject playerSurface;

    [SerializeField] AudioClip chargeSFX;
    [SerializeField] AudioClip chargeTickSFX;

    // Update is called once per frame
    void Update()
    {
        getChargeTimer(); //find the amount of charge
        
        if (currentChargeTimer > 0) //if there is any charge whatsoever
        {
            if (!chargeStarted) //this flag is in place to ensure the sound only starts once
            {
                AudioHelper.PlayClip2D(chargeSFX, 0.6f);
                chargeStarted = true;
            }
            
            if (currentChargeTimer >= 0.33f && firstFlash == false) //the bool ensures this only triggers once
            {
                StartCoroutine(flashCoroutine());
                firstFlash = true;
                AudioHelper.PlayClip2D(chargeTickSFX, 0.7f);
            }
            if (currentChargeTimer >= 0.66f && secondFlash == false) //similar to previous, though louder and adds the pop up text
            {
                StartCoroutine(flashCoroutine());
                secondFlash = true;
                AudioHelper.PlayClip2D(chargeTickSFX, 0.9f);
                FloatingTextController.CreateFloatingText("Max!", transform);
            }
        } else //when no longer charging, resets everything to zero
        { 
            firstFlash = false;
            secondFlash = false;
            chargeStarted = false;
            sfxClip = GameObject.Find("2D Audio ChargingSoundEffect (UnityEngine.AudioClip)");
            Destroy(sfxClip);
        }
    }

    void getChargeTimer()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        currentChargeTimer = playerMovement.returnChargeTimer();
    }

    IEnumerator flashCoroutine()
    {
        playerSurface.GetComponent<SkinnedMeshRenderer>().material = activatedMaterial;

        yield return new WaitForSeconds(0.1f);

        playerSurface.GetComponent<SkinnedMeshRenderer>().material = baseMaterial;
    }
}
