using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    private Vector3 startingOffset;
    private float minimumZoom = 0.5f;
    private float maximumZoom = 1.5f;
    private float currentZoom = 1.0f;
    private float zoomIncrement = 0.05f;
    private float scrollValue;
    public Component Player;

	// Use this for initialization
	void Start () {
        startingOffset = this.transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0) { currentZoom = Mathf.Max(currentZoom - zoomIncrement, minimumZoom); }
        else if (scrollValue < 0) { currentZoom = Mathf.Min(currentZoom + zoomIncrement, maximumZoom); }
        this.transform.position = Player.transform.position + startingOffset * currentZoom;
	}
}
