using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour {

    public int HP;
    public Text displayText;

	// Use this for initialization
	void Start () {
        UpdateDisplayText();
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

            HP -= damage;
            UpdateDisplayText();
            Destroy(collision.gameObject);
        }
    }

    private void UpdateDisplayText()
    {
        displayText.text = "Base HP: " + HP;
    }
}
