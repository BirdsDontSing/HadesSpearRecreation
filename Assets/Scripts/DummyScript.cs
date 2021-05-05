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

            int damage = hitboxScript.returnDamage();

            health -= damage;

            FloatingTextController.CreateFloatingText(damage.ToString(), transform);

            Debug.Log("Dummy is at " + health + " health");
        }
    }
}
