using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour, IDamageable, IKillable {

    GameObject target;
    NavMeshAgent navMeshAgent;
    GameObjectTracker gameObjectTracker;

    public int StartingHP;
    public int HP { get; set; }
    public Text DisplayText;
    public float FollowDistance;

    // Called once when object is initialized
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Use this for initialization
    void Start () {
        navMeshAgent.stoppingDistance = FollowDistance;
        navMeshAgent.autoBraking = true;
        HP = StartingHP;
        EventManager.RegisterSpawn(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        gameObjectTracker = GetComponentInChildren<GameObjectTracker>();
        if (gameObjectTracker.HasTargetInRange())
        {
            // Testing to see if clamping point they need to go to to y=0 plane gives them continuous speed.
            target = gameObjectTracker.GetTargetsInRange().First();
            Vector3 position = new Vector3(target.transform.position.x, 0.0f, target.transform.position.z);
            navMeshAgent.SetDestination(position);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject thingCollidedWith = collision.gameObject;
        if (thingCollidedWith.GetComponent<Tags>() != null && thingCollidedWith.GetComponent<Tags>().Contains("Bullet"))
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
