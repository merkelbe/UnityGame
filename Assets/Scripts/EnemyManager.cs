using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour, IDamageable, IKillable {

    internal GameObject Target;
    NavMeshAgent navMeshAgent;

    internal int StartingHP;
    public int HP { get; set; }
    internal Text DisplayText;
    internal float FollowDistance;

    // Called once when object is initialized
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EventManager.RegisterSpawn(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        navMeshAgent.stoppingDistance = FollowDistance;
        navMeshAgent.autoBraking = true;
        HP = StartingHP;
    }
	
	// Update is called once per frame
	void Update () {
        navMeshAgent.SetDestination(Target.transform.position);
        
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject thingCollidedWith = collision.gameObject;
        if (thingCollidedWith.CompareTag("Bullet"))
        {
            int damage = thingCollidedWith.GetComponent<BulletDamage>().damage;
            TakeDamage(damage);

            // Destroyed slightly after collision so you still get physics interactions from the collision.
            Destroy(thingCollidedWith, 0.04f);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        this.HP = 0;
        EventManager.RegisterDeath(this.gameObject);
        Destroy(this.gameObject);
    }
}
