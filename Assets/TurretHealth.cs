﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            int damage = collision.gameObject.GetComponent<BulletDamage>().damage;
            BulletDamage damageScript = collision.gameObject.GetComponent<BulletDamage>();
            damage = damageScript.damage;

            // Calls parent object to take damage.
            GetComponentInParent<TurretMain>().TakeDamage(damage);
            
            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }
}
