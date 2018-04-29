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
        if (gameObjectTracker.HasEnemyInRange())
        {
            GameObject enemy = gameObjectTracker.GetEnemyInRange();
            if(timer == 0)
            {
                FireAt(enemy);
                timer = CoolDownTime;
            }
        }

        if(timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        }
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
