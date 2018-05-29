using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectTracker : MonoBehaviour {
    
    public string TrackingTag;

    private float trackingRadius; // ASSUMPTION: Transform is in shape of a cylinder

    private List<GameObject> targetsInRange;


	// Use this for initialization
	void Start ()
    {
        trackingRadius = this.transform.localScale.x;
        RefreshTargets();
    }

    public void RefreshTargets()
    {
        targetsInRange = new List<GameObject>();
        GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag(TrackingTag);
        foreach (GameObject gameObject in allGameObjects)
        {
            if (TargetInRange(gameObject) && !targetsInRange.Contains(gameObject))
            {
                targetsInRange.Add(gameObject);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    public List<GameObject> GetTargetsInRange()
    {
        return targetsInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.CompareTag(TrackingTag))
        {
            if (!targetsInRange.Contains(entity))
            {
                targetsInRange.Add(entity);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.CompareTag(TrackingTag))
        {
            StopTrackingGameObject(entity);
        }
    }

    private void OnEnable()
    {
        EventManager.OnDeath += StopTrackingGameObject;
        EventManager.OnSpawn += StartTrackingGameObject;
    }

    private void OnDisable()
    {
        EventManager.OnDeath -= StopTrackingGameObject;
        EventManager.OnSpawn -= StartTrackingGameObject;
    }

    public void StopTrackingGameObject(GameObject gameObject)
    {
        if (targetsInRange.Contains(gameObject))
        {
            targetsInRange.Remove(gameObject);
        }
    }

    public void StartTrackingGameObject(GameObject gameObject)
    {
        if(gameObject.CompareTag(TrackingTag) && TargetInRange(gameObject) && !targetsInRange.Contains(gameObject))
        {
            targetsInRange.Add(gameObject);
        }
    }

    public bool TargetInRange(GameObject gameObject)
    {
        // Need to zero out y coordinate to make tracking area a cylinder to match the collider
        Vector3 turretXZPosition = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        Vector3 gameObjectXZPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);

        return Vector3.Distance(turretXZPosition, gameObjectXZPosition) <= trackingRadius;
    }
}
