using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class EnemySpawner : MonoBehaviour {

    public GameObject Enemy;
    public GameObject EnemyTarget;
    public Text EnemyHealthText;
    public Transform[] SpawnPositions;
    public float SpawnFrequency;
    public int StartingHP;
    public float FollowDistance; 

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
        for(int i = 0; i < SpawnPositions.Length; ++i)
        {
            Vector3 possibleSpawnPoint = SpawnPositions[i].transform.position;
            RaycastHit collision;
            Physics.SphereCast(possibleSpawnPoint, 0.9f, Vector3.zero, out collision);
            bool spaceEmpty = collision.collider == null;

            if (spaceEmpty)
            {
                GameObject enemy = Instantiate(Enemy, possibleSpawnPoint, Quaternion.identity);
                EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
                enemyManager.Target = EnemyTarget;
                enemyManager.DisplayText = EnemyHealthText;
                enemyManager.StartingHP = StartingHP;
                enemyManager.FollowDistance = FollowDistance;
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
