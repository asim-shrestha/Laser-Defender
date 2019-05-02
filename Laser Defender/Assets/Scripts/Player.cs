using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	//Configuration
	[Header("Player Configuration")]
	[SerializeField] int playerHealth = 3;
	[SerializeField] float playerSpeed = 10f;
	[SerializeField] float screenPadding = 1f;

	[Header("Projectile")]
	[SerializeField] GameObject laserPrefab;
	[SerializeField] float laserSpeed = 10f;
	[SerializeField] float fireDelay = 0.2f;

	//Sprites
	[Header("Sprites")]
	[SerializeField] Sprite leftSprite;
	[SerializeField] Sprite middleSprite;
	[SerializeField] Sprite rightSprite;

	private float XMin;
	private float XMax;

	private float YMin;
	private float YMax;

	private Boolean isFiring = false;

	// Start is called before the first frame update
	void Start() {
		SetUpBoundaries();
	}

	// Update is called once per frame
	void Update() {
		MovePlayer();
		Fire();
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
		GetSprite();

		//Changes position based on unity's predefine axis'
		//This involves buttons such as left/right and up/down

		//Get X, Y positions within boundary
		float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
		float newXPos = Mathf.Clamp(transform.position.x + deltaX, XMin, XMax);
		float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
		float newYPos = Mathf.Clamp(transform.position.y + deltaY, YMin, YMax);

		transform.position = new Vector2(newXPos, newYPos);
	}

	private void GetSprite() {
		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();

		//Get ship sprite
		float inputAxis = Input.GetAxis("Horizontal");
		if (inputAxis < -0.4) spriteRenderer.sprite = leftSprite;
		else if (inputAxis > 0.4) spriteRenderer.sprite = rightSprite;
		else spriteRenderer.sprite = middleSprite;
	}

	private void Fire() {
		//If space is held, fire lasers
		if (Input.GetButton("Fire1")) {
			if (!isFiring) {
				isFiring = true;
				StartCoroutine(FireContinuously());
			}
		}
	}

	IEnumerator FireContinuously() {
		//Runs until spacebar is released
		GameObject laser = Instantiate(
			laserPrefab,
			transform.position,
			Quaternion.identity);
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Math.Max(playerSpeed, laserSpeed));

		//Ship recoil
		float recoilOffset = 0.08f;
		float newYPos = transform.position.y - recoilOffset;	//Recoil position
		transform.position = new Vector2(transform.position.x, newYPos);
		float recoilDelay = 0.05f;								
		yield return new WaitForSeconds(recoilDelay);
		newYPos = transform.position.y + recoilOffset;			//Reset position
		transform.position = new Vector2(transform.position.x, transform.position.y + recoilOffset);

		//Wait between successive shots
		yield return new WaitForSeconds(fireDelay - recoilDelay);
		isFiring = false;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

		//If damageDealer is null, return
		if (!damageDealer) { return; }

		this.HandleDamage(damageDealer.GetDamage());
		damageDealer.HandleHit();
	}

	private void HandleDamage(int damage) {
		playerHealth -= damage;

		if (playerHealth <= 0) {
			Destroy(this.gameObject);
		}
	}
}
