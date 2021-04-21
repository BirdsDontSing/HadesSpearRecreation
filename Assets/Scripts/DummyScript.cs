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
            health -= 25;

            Debug.Log("Dummy is at " + health + " health");
        }
    }
}
