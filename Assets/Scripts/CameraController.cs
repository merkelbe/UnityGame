using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject Player;

    private Vector3 baseOffset;
    private Vector3 rotatedOffset;
    private Vector3 cameraPosition;
    private Vector3 cameraToPlayerDirection;
    private RaycastHit hit;
    private Ray ray;
    

    // Use this for initialization
    void Start () {
        baseOffset = this.transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        // Figures out position of camera
        Quaternion playerRoatation = Quaternion.Euler(0, Player.transform.rotation.eulerAngles.y, 0.0f);
        rotatedOffset =  playerRoatation * baseOffset;
        
        // Figures out if camera is in another object and needs to zoom in on player.
        for(int i = 0; i < 20; ++i)
        {
            float zoom = 1 - (0.05f * i);
            cameraPosition = Player.transform.position + rotatedOffset * zoom;
            
            cameraToPlayerDirection = Player.transform.position - cameraPosition;
            ray = new Ray(cameraPosition, cameraToPlayerDirection);
            if(Physics.Raycast(ray,out hit))
            {
                // This means there's nothing in the way of the camera to the player.
                if(hit.collider.gameObject == Player || Player.GetComponent<PlayerManager>().HP <= 0)
                {
                    break;
                }
            }
        }
        
        this.transform.SetPositionAndRotation(cameraPosition, playerRoatation);
    }
}
