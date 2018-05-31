using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour, IDamageable,IKillable {

    public int StartingHP;
    public int HP { get; set; }

	// Use this for initialization
	void Start () {
        HP = StartingHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        HP = 0;
        Destroy(this.gameObject,0.01f);
        EventManager.RegisterDeath(this.gameObject);
    }

}
