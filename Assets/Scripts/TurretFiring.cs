using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFiring : MonoBehaviour {

    public float CoolDownTime = 5;
    public GameObject Bullet;

    private float timer;


	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameObjectTracker gameObjectTracker = GetComponentInChildren<GameObjectTracker>();
        if (gameObjectTracker.HasTargetInRange())
        {
            //GameObject enemy = gameObjectTracker.GetTargetInRange();
            //if(timer == 0)
            //{
            //    if (ClearShotAtEnemy(enemy))
            //    {
            //        FireAt(enemy);
            //        timer = CoolDownTime;
            //    }
            //}
            
            if(timer == 0)
            {
                List<GameObject> possibleEnemies = gameObjectTracker.GetTargetsInRange();
                foreach(GameObject enemy in possibleEnemies)
                {
                    if (ClearShotAtEnemy(enemy))
                    {
                        FireAt(enemy);
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

    private bool ClearShotAtEnemy(GameObject enemy)
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
        // TODO: Figure out bullet position and orientation
        Vector3 fireDirection = enemy.transform.position - this.transform.position;
        Vector3 normalizedFireDirection = Vector3.Normalize(fireDirection);
        Vector3 bulletSpawnPosition = this.transform.position + normalizedFireDirection * 2.0f;
        //Quaternion bulletOrientation = Quaternion.LookRotation(fireDirection,Vector3.forward);
        Quaternion bulletOrientation = Quaternion.FromToRotation(Vector3.up, fireDirection);
        GameObject bulletCopy = GameObject.Instantiate(Bullet, bulletSpawnPosition, bulletOrientation);
        bulletCopy.GetComponent<BulletMovement>().Fire();
    }
}
