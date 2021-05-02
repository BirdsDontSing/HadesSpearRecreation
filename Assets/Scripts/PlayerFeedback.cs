using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{

    float currentChargeTimer;
    bool firstFlash = false;
    bool secondFlash = false;


    [SerializeField] Material activatedMaterial;
    [SerializeField] Material baseMaterial;
    [SerializeField] GameObject playerSurface;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getChargeTimer();
        
        if (currentChargeTimer > 0)
        {
            if (currentChargeTimer >= 0.33f && firstFlash == false)
            {
                StartCoroutine(flashCoroutine());
                firstFlash = true;
            }
            if (currentChargeTimer >= 0.66f && secondFlash == false)
            {
                StartCoroutine(flashCoroutine());
                secondFlash = true;
            }
        } else
        {
            firstFlash = false;
            secondFlash = false;
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
