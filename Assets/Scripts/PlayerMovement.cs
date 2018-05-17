using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject bullet;
    public float movementForce = 50;
    public float mouseSensitivity = 1;
    public float reorientationTime = 5;

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

    // Orientation variables 
    private float timeDisoriented;
    private float epsilon = 0.01f;
    private float rotationSpeed;
    private float xAngle;
    private float newXAngle;
    private float zAngle;
    private float newZAngle;
    private Quaternion newRotationAngle;
    private bool alive;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        bulletOffset = new Vector3(0, 0, 1);
        timeDisoriented = 0;
        
        //TODO: find a better place for this
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update ()
    {
        alive = GetComponent<PlayerHealth>().CurrentHP > 0;

        if (alive)
        {

            // Rotates player based on mouse's movements in the X direction
            mouseXMovement = Input.GetAxis("Mouse X");
            this.transform.Rotate(0.0f, mouseXMovement * mouseSensitivity, 0.0f);

            // Clamping to the y = 1 plane
            rigidBody.transform.Translate(0.0f, (1.0f - this.transform.position.y), 0.0f);

            // Player movement based on WASD input
            horizontalMovement = Input.GetAxis("Horizontal");
            verticalMovement = Input.GetAxis("Vertical");
            normalizer = Mathf.Sqrt(horizontalMovement * horizontalMovement + verticalMovement * verticalMovement);

            if (normalizer > 0)
            {
                playerRotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
                movementVector = new Vector3(horizontalMovement * movementForce * Time.deltaTime / normalizer, 0, verticalMovement * movementForce * Time.deltaTime / normalizer) * 100;
                rigidBody.AddForce(playerRotation * movementVector);
            }

            // Reorients player if they're not orthogonal to the y = 1 plane.
            if (isDisoriented())
            {
                timeDisoriented += Time.deltaTime;
                if (timeDisoriented > reorientationTime)
                {
                    rotationSpeed = 4.0f / 5.0f;
                    xAngle = transform.rotation.eulerAngles.x;
                    newXAngle = xAngle > 180 ? 360 * (1 - rotationSpeed) + rotationSpeed * xAngle : rotationSpeed * xAngle;
                    zAngle = transform.rotation.eulerAngles.z;
                    newZAngle = zAngle > 180 ? 360 * (1 - rotationSpeed) + rotationSpeed * zAngle : rotationSpeed * zAngle;

                    newRotationAngle = Quaternion.Euler(new Vector3(newXAngle, transform.rotation.eulerAngles.y, newZAngle));
                    this.transform.SetPositionAndRotation(this.transform.position, newRotationAngle);

                    if (!isDisoriented())
                    {
                        timeDisoriented = 0;
                    }
                }
            }

            // Spawns bullet when left click
            bool leftClick = Input.GetMouseButtonDown(0);
            if (leftClick)
            {
                bulletRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
                bulletCopy = GameObject.Instantiate(bullet, this.transform.position + playerRotation * bulletOffset, bulletRotation);
                bulletCopy.GetComponent<BulletMovement>().Fire();

                Destroy(bulletCopy, 2.5f);
            }


        }
    }

    bool isDisoriented()
    {
        return Mathf.Abs(transform.rotation.eulerAngles.x) > epsilon || Mathf.Abs(transform.rotation.eulerAngles.z) > epsilon;
    }
}
