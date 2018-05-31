using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Tags>() != null && collision.gameObject.GetComponent<Tags>().Contains("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
