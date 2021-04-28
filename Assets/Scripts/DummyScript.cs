using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{

    int health = 10000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            HitboxScript hitboxScript = FindObjectOfType<HitboxScript>();

            health -= hitboxScript.returnDamage();

            Debug.Log("Dummy is at " + health + " health");
        }
    }
}
