using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] float playerSpeed;
	[SerializeField] float screenPadding;

	float XMin;
	float XMax;

	float YMin;
	float YMax;

    // Start is called before the first frame update
    void Start() {
		SetUpBoundaries();
	}

    // Update is called once per frame
    void Update() {
        MovePlayer();
    }

	private void SetUpBoundaries() {
		Camera gameCamera = Camera.main;

		//Get positions of the screen corners
		XMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + screenPadding;
		XMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - screenPadding;

		YMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + screenPadding;
		YMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - screenPadding;
	}

    private void MovePlayer() {
        //Changes position based on unity's predefine axis'
        //This involves buttons such as left/right and up/down

		//Get X Position within boundary
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
		float newXPos = Mathf.Clamp(transform.position.x + deltaX, XMin, XMax);

		//Get Y Position within boundary
		float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
		float newYPos = Mathf.Clamp(transform.position.y + deltaY, YMin, YMax);

		//Set Position
        transform.position = new Vector2(newXPos, newYPos);
    }
}
