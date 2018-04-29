using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject bullet;
    public float movementForce = 50;
    public float mouseSensitivity = 1;

    private float mouseXMovement;
    private Rigidbody rigidBody;
    private float horizontalMovement;
    private float verticalMovement;
    private float normalizer;
    private Quaternion playerRotation;
    private Quaternion bulletRotation;
    private GameObject bulletCopy;
    private Vector3 movementVector;
    private Vector3 bulletOffset;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        bulletOffset = new Vector3(0, 0, 1);

        //TODO: find a better place for this
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update ()
    {
        // Rotates player based on mouse's movements in the X direction
        float mouseXMovement = Input.GetAxis("Mouse X");
        this.transform.Rotate(0.0f, mouseXMovement * mouseSensitivity, 0.0f);
        
        // Clamping to the y = 1 plane
        rigidBody.transform.Translate(0.0f, (1.0f - this.transform.position.y), 0.0f);
        
        // Player movement based on WASD input
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        normalizer = Mathf.Sqrt(horizontalMovement * horizontalMovement + verticalMovement * verticalMovement);

        playerRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        movementVector = new Vector3(horizontalMovement * movementForce * Time.deltaTime / normalizer, 0, verticalMovement * movementForce * Time.deltaTime / normalizer) * 100;
        rigidBody.AddForce(playerRotation * movementVector);

        // Spawns bullet when left click
        bool leftClick = Input.GetMouseButtonDown(0);
        if (leftClick)
        {
            bulletRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            bulletCopy = GameObject.Instantiate(bullet,this.transform.position + playerRotation * bulletOffset, bulletRotation);
            bulletCopy.GetComponent<BulletMovement>().Fire();

            //float fireSpeed = bulletCopy.GetComponent<BulletMovement>().speed;
            //Vector3 fireDirection = Quaternion.Euler(0, bulletCopy.transform.rotation.eulerAngles.y, 0) * new Vector3(0, 0, 1);
            //Rigidbody rigidBody = bulletCopy.GetComponent<Rigidbody>();
            //rigidBody.AddForce(fireDirection * fireSpeed);

            Destroy(bulletCopy, 10);
        }
    }
}
