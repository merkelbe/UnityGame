using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectTracker : MonoBehaviour {
    
    public string TrackingTag;

    private float trackingRadius; // ASSUMPTION: Transform is in shape of a cylinder

    // Currently will sort targets by distance (closest to farthest away)
    private List<GameObject> targetsInRange;
    private DistanceSorter distanceSorter;

    private void Awake()
    {
        targetsInRange = new List<GameObject>();
        trackingRadius = this.transform.localScale.x;
        distanceSorter = new DistanceSorter(this.transform.position);
    }

    // Use this for initialization
    void Start ()
    {
        RefreshTargets();
    }

    public void RefreshTargets()
    {
        targetsInRange.Clear();
        GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag(TrackingTag);
        foreach (GameObject gameObject in allGameObjects)
        {
            if (TargetInRange(gameObject) && !targetsInRange.Contains(gameObject))
            {
                targetsInRange.Add(gameObject);
            }
        }
        sortTargets();
    }


    void sortTargets()
    {
        distanceSorter.CurrentPosition = this.transform.position;
        targetsInRange.Sort(distanceSorter);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public bool HasTargetInRange()
    {
        return targetsInRange.Count > 0;
    }

    public List<GameObject> GetTargetsInRange()
    {
        sortTargets();
        return targetsInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.GetComponent<Tags>()!= null && entity.GetComponent<Tags>().Contains(TrackingTag))
        {
            if (!targetsInRange.Contains(entity))
            {
                targetsInRange.Add(entity);
                sortTargets();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject entity = other.gameObject;
        if (entity.GetComponent<Tags>() != null && entity.GetComponent<Tags>().Contains(TrackingTag))
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
        if(gameObject.GetComponent<Tags>() != null && gameObject.GetComponent<Tags>().Contains(TrackingTag) && TargetInRange(gameObject) && !targetsInRange.Contains(gameObject))
        {
            targetsInRange.Add(gameObject);
            sortTargets();
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

// Sorts game objects by distance they are away from this game object.
public class DistanceSorter : IComparer<GameObject>
{
    public Vector3 CurrentPosition;

    public DistanceSorter(Vector3 _currentPosition)
    {
        CurrentPosition = _currentPosition;
    }

    public int Compare(GameObject _firstGameObject, GameObject _secondGameObject)
    {
        // Want closer objects earlier in gameObject list
        return (int)(Vector3.Distance(CurrentPosition, _firstGameObject.transform.position) - Vector3.Distance(CurrentPosition, _secondGameObject.transform.position));
    }
}

