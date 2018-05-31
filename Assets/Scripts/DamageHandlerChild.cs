using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandlerChild : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Tags>() != null && collision.gameObject.GetComponent<Tags>().Contains("Bullet"))
        {
            int damage = collision.gameObject.GetComponent<BulletDamage>().damage;
            BulletDamage damageScript = collision.gameObject.GetComponent<BulletDamage>();
            damage = damageScript.damage;

            // Calls parent object to take damage.
            GetComponentInParent<IDamageable>().TakeDamage(damage);
            
            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }
}
