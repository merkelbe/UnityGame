using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

    [Range(200,5000)]
    public float Speed;
    
    // Start is called once on initialization
    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        
	}

    public void Fire()
    {
        Vector3 fireDirection = Vector3.Normalize(Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) * Vector3.up);
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(fireDirection * Speed);
    }
}
