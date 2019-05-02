using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	//Configuration
	[SerializeField] float playerSpeed = 10f;
	[SerializeField] float screenPadding = 1f;
	[SerializeField] GameObject laserPrefab;
	[SerializeField] float laserSpeed = 10f;

	//Sprites
	[SerializeField] Sprite leftSprite;
	[SerializeField] Sprite middleSprite;
	[SerializeField] Sprite rightSprite;

	private float XMin;
	private float XMax;

	private float YMin;
	private float YMax;

    // Start is called before the first frame update
    void Start() {
		SetUpBoundaries();
	}

    // Update is called once per frame
    void Update() {
        MovePlayer();
		ShootLaser();
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

		getSprite();

		//Get X position within boundary
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
		float newXPos = Mathf.Clamp(transform.position.x + deltaX, XMin, XMax);

		//Get Y position within boundary
		float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
		float newYPos = Mathf.Clamp(transform.position.y + deltaY, YMin, YMax);

		//Set position
        transform.position = new Vector2(newXPos, newYPos);
    }

	private void getSprite() {
		//Get ship sprite
		float inputAxis = Input.GetAxis("Horizontal");
		if (inputAxis < 0) this.GetComponent<SpriteRenderer>().sprite = leftSprite;
		else if (inputAxis > 0) this.GetComponent<SpriteRenderer>().sprite = rightSprite;
		else this.GetComponent<SpriteRenderer>().sprite = middleSprite;
	}

	private void ShootLaser() {
		//If space is pressed, create a laser instance
		if (Input.GetButtonDown("Fire1")) {
			GameObject laser = Instantiate(
				laserPrefab,
				transform.position,
				Quaternion.identity);

			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Math.Max(playerSpeed,laserSpeed));
		}
	}

}
