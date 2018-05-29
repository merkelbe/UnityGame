using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class TurretFiring : MonoBehaviour {

    public float CoolDownTime = 5;
    public GameObject Bullet;

    // Difficulty of Turret from 0 to 1.  
    // Zero shoots at where they are right now and 
    // 1 shoots at where they will be if they keep the current velocity.
    [Range(0, 1)]
    public float TurretDifficulty;

    private float bulletVelocity;
    
    private float timer;

    private Vector3 turretPosition;
    private Vector3 enemyPosition;
    private Vector3 enemyVelocity;

    // Used to figure out where to shoot to hit the enemy
    private float a;
    private float b;
    private float c;


	// Use this for initialization
	void Start () {
        turretPosition = new Vector3();
        enemyPosition = new Vector3();
        enemyVelocity = new Vector3();

        timer = 0;
        bulletVelocity = Bullet.GetComponent<BulletMovement>().Speed * Time.fixedDeltaTime / Bullet.GetComponent<Rigidbody>().mass;
    }
	
	// Update is called once per frame
	void Update () {
        GameObjectTracker gameObjectTracker = GetComponentInChildren<GameObjectTracker>();
        bool hasTargetInRange = gameObjectTracker.GetTargetsInRange().Count > 0;
        if (hasTargetInRange)
        {
            if(timer == 0)
            {
                List<GameObject> possibleEnemies = gameObjectTracker.GetTargetsInRange();
                
                for (int i = 0; i < possibleEnemies.Count; ++i)
                {
                    GameObject enemy = possibleEnemies[i];

                    if (clearShotAtEnemy(enemy))
                    {
                        turretPosition = this.transform.position;
                        enemyPosition = enemy.transform.position;
                        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
                        enemyVelocity = navMeshAgent != null ? navMeshAgent.velocity : enemy.GetComponent<Rigidbody>().velocity;

                        a = Mathf.Pow(Vector3.Magnitude(enemyVelocity),2) - Mathf.Pow(bulletVelocity, 2);
                        b = 2 * (enemyPosition.x * enemyVelocity.x - turretPosition.x * enemyVelocity.x + enemyPosition.y * enemyVelocity.y - turretPosition.y * enemyVelocity.y + enemyPosition.z * enemyVelocity.z - turretPosition.z * enemyVelocity.z);
                        c = Mathf.Pow(Vector3.Distance(enemyPosition, turretPosition),2);

                        List<float> solutions = solveQuadratic(a, b, c);
                        if(solutions.Count > 0)
                        {
                            float solution = solutions.Max();
                            Vector3 enemyFuturePosition = enemyPosition + solution * TurretDifficulty * enemyVelocity;
                            FireAt(enemyFuturePosition);
                        }
                        
                        timer = CoolDownTime;
                        break;
                    }
                }
            }
        }

        if(timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        }
	}

    private bool clearShotAtEnemy(GameObject enemy)
    {
        RaycastHit hit;
        Vector3 fireDirection = enemy.transform.position - this.transform.position;
        Vector3 bulletSpawnPosition = this.transform.position + 2.0f * Vector3.Normalize(fireDirection);
        Ray ray = new Ray(bulletSpawnPosition, fireDirection);
        
        float bulletRadius = Bullet.transform.localScale.x;

        if (Physics.SphereCast(ray,bulletRadius,out hit))
        {
            if(hit.collider.gameObject == enemy)
            {
                return true;
            }
        }
        return false;
    }

    private void FireAt(GameObject enemy)
    {
        FireAt(enemy.transform.position);
    }

    private void FireAt(Vector3 position)
    {
        Vector3 fireDirection = position - this.transform.position;
        Vector3 normalizedFireDirection = Vector3.Normalize(fireDirection);
        Vector3 bulletSpawnPosition = this.transform.position + normalizedFireDirection * 2.0f;
        Quaternion bulletOrientation = Quaternion.FromToRotation(Vector3.up, fireDirection);
        GameObject bulletCopy = GameObject.Instantiate(Bullet, bulletSpawnPosition, bulletOrientation);
        bulletCopy.GetComponent<BulletMovement>().Fire();
    }

    // Solves for x in equation ax^2 + bx + c = 0.  Returns only real solutions.
    private List<float> solveQuadratic(float a,float b, float c)
    {
        List<float> solutions = new List<float>();
        float discriminantSquared = b * b - 4 * a * c;

        // One solution
        if (discriminantSquared == 0)
        {
            solutions.Add( -b / 2 / a);
        }

        // Two solutions
        else if(discriminantSquared > 0)
        {
            float discriminant = Mathf.Sqrt(discriminantSquared);
            solutions.Add((-b + discriminant) / 2 / a);
            solutions.Add((-b - discriminant) / 2 / a);
        }

        // No solutions if discriminant squared is less than 0

        return solutions;
    }
}
