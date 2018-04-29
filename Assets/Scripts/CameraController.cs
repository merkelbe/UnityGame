using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 baseOffset;
    private Vector3 rotatedOffset;
    public Component Player;
    

    // Use this for initialization
    void Start () {
        baseOffset = this.transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion playerRoatation = Quaternion.Euler(0, Player.transform.rotation.eulerAngles.y, 0.0f);
        rotatedOffset =  playerRoatation * baseOffset;
        this.transform.SetPositionAndRotation(Player.transform.position + rotatedOffset, playerRoatation);
    }
}
