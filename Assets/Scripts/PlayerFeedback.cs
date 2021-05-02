using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{

    float currentChargeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getChargeTimer();


    }

    void getChargeTimer()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        currentChargeTimer = playerMovement.returnChargeTimer();
    }
}
