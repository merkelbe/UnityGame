using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTracker : MonoBehaviour {
    
    public string enemyTag = "Hero Team";
    public int maxScale = 10; // When turret is instantiated, perimeter grows from nothing to max scale in order to hit entities already in the perimeter upon in

    private int scaleReset = 2;
    private List<GameObject> enemiesInRange;
    private bool growing;
    private float precision;

	// Use this for initialization
	void Start () {
        enemiesInRange = new List<GameObject>();
        growing = false;
        precision = 0.1f;
        GrowPerimeter();
	}
	
	// Update is called once per frame
	void Update () {
        // Handles growing of hit box until full size
        if (growing)
        {
            transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + precision, maxScale), transform.localScale.y, Mathf.Min(transform.localScale.z + precision, maxScale));
            if(transform.localScale.x >= maxScale)
            {
                growing = false;
            }
        }
	}

    public bool HasEnemyInRange()
    {
        return enemiesInRange.Count > 0;
    }

    public GameObject GetEnemyInRange()
    {
        return enemiesInRange[0];
    }

    private void GrowPerimeter()
    {
        growing = true;
        transform.localScale = new Vector3(scaleReset, transform.localScale.y, scaleReset);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.CompareTag(enemyTag))
        {
            if (!enemiesInRange.Contains(entity))
            {
                enemiesInRange.Add(entity);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(entity))
            {
                enemiesInRange.Remove(entity);
            }
        }
    }
}
