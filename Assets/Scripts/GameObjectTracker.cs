using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTracker : MonoBehaviour {
    
    public string TrackingTag = "Hero Team";
    public int MaxScale = 10; // When turret is instantiated, perimeter grows from nothing to max scale in order to hit entities already in the perimeter upon in
    public bool growPerimeter;

    private int scaleReset = 2;
    private List<GameObject> targetsInRange;
    private bool growing;
    private float precision;

	// Use this for initialization
	void Start () {
        targetsInRange = new List<GameObject>();
        growing = false;
        precision = 0.1f;
        if (growPerimeter)
        {
            GrowPerimeter();
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Handles growing of hit box until full size
        if (growing)
        {
            transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + precision, MaxScale), transform.localScale.y, Mathf.Min(transform.localScale.z + precision, MaxScale));
            if(transform.localScale.x >= MaxScale)
            {
                growing = false;
            }
        }
	}

    public bool HasTargetInRange()
    {
        return targetsInRange.Count > 0;
    }

    public GameObject GetTargetInRange()
    {
        return targetsInRange[0];
    }

    public List<GameObject> GetTargetsInRange()
    {
        return targetsInRange;
    }

    private void GrowPerimeter()
    {
        growing = true;
        transform.localScale = new Vector3(scaleReset, transform.localScale.y, scaleReset);
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
            if (targetsInRange.Contains(entity))
            {
                targetsInRange.Remove(entity);
            }
        }
    }
}
