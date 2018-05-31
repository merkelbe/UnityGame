using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class EnemySpawner : MonoBehaviour {

    public GameObject[] Enemies;
    public Text EnemyHealthText;
    public Transform[] SpawnPositions;
    public float SpawnFrequency;
    public int StartingHP;
    public float FollowDistance;
    public GameObject EnemyBullet;
    public string EnemyTrackingTag;
    public float EnemyShootingRange;
    public float EnemyCoolDownTime;
    [Range(0, 1)]
    public float EnemyShootingSkill;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", SpawnFrequency, SpawnFrequency);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        shuffle(ref SpawnPositions);
        shuffle(ref Enemies);
        for(int i = 0; i < SpawnPositions.Length; ++i)
        {
            Vector3 possibleSpawnPoint = SpawnPositions[i].transform.position;
            RaycastHit collision;
            Physics.SphereCast(possibleSpawnPoint, 0.9f, Vector3.zero, out collision);
            bool spaceEmpty = collision.collider == null;

            if (spaceEmpty)
            {
                // Enemy initializations
                GameObject enemy = Instantiate(Enemies[0], possibleSpawnPoint, Quaternion.identity);
                //EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
                //enemyManager.DisplayText = EnemyHealthText;
                //enemyManager.StartingHP = StartingHP;
                //enemyManager.FollowDistance = FollowDistance;
                //ProjectileFiring projectileFiring = enemy.GetComponent<ProjectileFiring>();
                //projectileFiring.CoolDownTime = EnemyCoolDownTime;
                //projectileFiring.ShootingRange = EnemyShootingRange;
                //projectileFiring.ShootingSkill = EnemyShootingSkill;
                //GameObjectTracker gameObjectTracker = enemy.GetComponentInChildren<GameObjectTracker>();
                //gameObjectTracker.TrackingTag = EnemyTrackingTag;
                EventManager.RegisterSpawn(enemy);
                break;
            }
        }
    }

    private void shuffle<T>(ref T[] array)
    {
        T[] shuffledArray = array.OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();
        array = shuffledArray;
    }
}
