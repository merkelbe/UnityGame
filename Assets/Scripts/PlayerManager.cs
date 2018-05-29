using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IDamageable, IKillable {
    
    // Player movement variables - WASD movement
    public float movementForce = 50;

    private Rigidbody rigidBody;
    private float horizontalMovement;
    private float verticalMovement;
    private float normalizer;
    private Quaternion playerRotation;
    private Quaternion bulletRotation;
    private Vector3 movementVector;

    // Player movement variables - Player/Camera orientation
    public float reorientationTime = 5;
    public float mouseSensitivity = 1;

    private float mouseXMovement;
    private float timeDisoriented;
    private float epsilon = 0.01f;
    private float rotationSpeed;
    private float xAngle;
    private float newXAngle;
    private float zAngle;
    private float newZAngle;
    private Quaternion newRotationAngle;

    // Player health variables
    public int StartingHP;
    public int HP { get; set; }
    public Text DisplayText;
    public float RespawnTime;

    private float timer;
    private Vector3 respawnPosition;
    private Quaternion respawnOrientation;
    
    // Bullet variables
    public GameObject bullet;

    private GameObject bulletCopy;
    private Vector3 bulletOffset;
    

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        timeDisoriented = 0;

        HP = StartingHP;
        respawnPosition = new Vector3(0.0f, 1.0f, 0.0f);
        respawnOrientation = Quaternion.identity;
        updateDisplayText();

        bulletOffset = new Vector3(0, 0, 1);

        //TODO: find a better place for this
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update ()
    {
        // If player is alive
        if (HP > 0)
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
            playerRotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);

            if (normalizer > 0)
            {
                movementVector = new Vector3(horizontalMovement * movementForce * Time.deltaTime / normalizer, 0, verticalMovement * movementForce * Time.deltaTime / normalizer) * 100;
                rigidBody.AddForce(playerRotation * movementVector);
            }

            // Reorients player if they're not orthogonal to the y = 1 plane.
            // Gradually reorients player after 'reorientationTime' seconds at the speed of 'rotationSpeed'
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

            // Bullet spawning and firing when player uses left-click
            bool leftClick = Input.GetMouseButtonDown(0);
            if (leftClick)
            {
                bulletRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
                bulletCopy = GameObject.Instantiate(bullet, this.transform.position + playerRotation * bulletOffset, bulletRotation);
                bulletCopy.GetComponent<BulletMovement>().Fire();

                Destroy(bulletCopy, 2.5f);
            }


        }


        // Handles respawn timing logic for player.
        // Assumption when player dies, timer is set to desired respawn time.
        if(timer > 0)
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
            if(timer == 0)
            {
                respawn();
            }
            updateDisplayText();
        }
    }

    ///////////////////////
    // Interface Methods //
    ///////////////////////

    public void Kill()
    {
        // Forces HP to 0 when killing the object
        // Update logic assumes player is dead now.
        HP = 0;

        // Stops object from moving
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Makes object invisible and takes away its collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        // Registers death with event manager so other objects can react accordingly
        EventManager.RegisterDeath(this.gameObject);

        // Sets timer to respawn time.  Timer will count down and then respawn object when timer hits 0.
        timer = RespawnTime;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        updateDisplayText();
    }

    /////////////////////
    // Private Methods //
    /////////////////////

    private void OnCollisionEnter(Collision collision)
    {
        GameObject thingCollidedWith = collision.gameObject;
        if (thingCollidedWith.CompareTag("Bullet"))
        {
            int damage = thingCollidedWith.GetComponent<BulletDamage>().damage;
            TakeDamage(damage);
            Destroy(thingCollidedWith, 0.04f); // This is to get a little bit of the physical force from the object before you destroy it.

            if(HP <=0 && timer == 0)
            {
                Kill();
            }
        }

        else if(thingCollidedWith.name == "Enemy" || thingCollidedWith.name == "Enemy(Clone)")
        {
            Kill();
        }
    }

    bool isDisoriented()
    {
        return Mathf.Abs(transform.rotation.eulerAngles.x) > epsilon || Mathf.Abs(transform.rotation.eulerAngles.z) > epsilon;
    }
    void respawn()
    {
        // Respawns player
        this.transform.SetPositionAndRotation(respawnPosition, respawnOrientation);
        HP = StartingHP;
        
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;

        // Registers spawn with event manager so other objects update accordingly.
        EventManager.RegisterSpawn(this.gameObject);
    }

    void updateDisplayText()
    {
        if(HP > 0)
        {
            DisplayText.text = "Player Health: " + HP;
        }
        else
        {
            DisplayText.text = string.Format("Respawn Time: {0:0.00}", timer);
        }
    }
}
