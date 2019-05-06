using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	//Configuration
	[Header("Player Configuration")]
	[SerializeField] bool isStarted = false;
	[SerializeField] bool isInvincible = false;
	[SerializeField] int startingPlayerHealth = 3;
	[SerializeField] float playerSpeed = 10f;
	[SerializeField] float screenPadding = 1f;
	[SerializeField] GameObject explosionParticles;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip hitsounds;

	[Header("Projectile")]
	[SerializeField] List<GameObject> laserPrefabs;
	[SerializeField] int laserIndex = 0;
	[SerializeField] float originalLaserSpeed = 10f;
	[SerializeField] float laserSpeed = 0f;
	[SerializeField] float originalFireDelay = 0.2f;
	[SerializeField] float fireDelay = 0f;

	//Sprites
	[Header("Sprites")]
	[SerializeField] Sprite leftSprite;
	[SerializeField] Sprite middleSprite;
	[SerializeField] Sprite rightSprite;

	private float XMin;
	private float XMax;

	private float YMin;
	private float YMax;

	private int playerHealth;
	private Vector3 originalPosition;
	private bool isStartAnimation = false;
	private bool isFiring = false;

	public void StartPlayer() {
		GetComponent<Transform>().position = originalPosition;
		playerHealth = startingPlayerHealth;
		gameObject.SetActive(true);
		this.GetComponent<SpriteRenderer>().sprite = middleSprite;
		laserIndex = 0;
		laserSpeed = originalLaserSpeed;
		fireDelay = originalFireDelay;
		isStarted = true;
		isStartAnimation = true;
		isFiring = false;
		FindObjectOfType<GameSession>().SetHealth(playerHealth);
		SetUpBoundaries();
	}

	void Start() {
		originalPosition = this.GetComponent<Transform>().position;
		gameObject.SetActive(false);
	}


	// Update is called once per frame
	void Update() {
		if(!isStarted) { return; }

		if (isStartAnimation) {
			StartAnimation();

			return;
		}

		MovePlayer();
		Fire();
	}

	private void StartAnimation() {
		Vector3 gameStartPosition = new Vector3(transform.position.x, -4f, 0);
		float movementThisFrame = (playerSpeed / 2f) * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, gameStartPosition, movementThisFrame);

		if (transform.position == gameStartPosition) {
			isStartAnimation = false;
		}
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
		float spriteThreshold = 0.3f;

		//Get ship sprite
		float inputAxis = Input.GetAxis("Horizontal");
		if (inputAxis < -spriteThreshold) spriteRenderer.sprite = leftSprite;
		else if (inputAxis > spriteThreshold) spriteRenderer.sprite = rightSprite;
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
			laserPrefabs[laserIndex],
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
		if (collision.tag == "PowerUp") {
			collision.gameObject.GetComponent<DamageDealer>().HandleHit();
			PowerUpLaser();
			return;
		}

		DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

		//If damageDealer is null, return
		if (!damageDealer) {
			//If it is not a damageDealer (laser) and is an enemy instead, deal damage
			int enemyCollisionDamage = 1;
			if (collision.gameObject.GetComponent<Enemy>()) { HandleDamage(enemyCollisionDamage); }
			return;
		}

		this.HandleDamage(damageDealer.GetDamage());
		damageDealer.HandleHit();
	}

	private void PowerUpLaser() {
		//Upgrade laser
		laserIndex++;

		//Fix index if we are out of range
		if(laserIndex >= laserPrefabs.Count) {
			laserIndex--;
		}

		playerSpeed += 1f;
		laserSpeed += 1f;
		fireDelay -= 0.025f;

		//Cap speeds
		if(playerSpeed > 10f) {
			playerSpeed = 10f;
			laserSpeed = 10f;
		}

		//Make sure fireDelay is more than 0
		if(fireDelay < 0) {
			fireDelay = 0;
		}
	}

	private void HandleDamage(int damage) {
		if(isInvincible) { return; }

		playerHealth -= damage;
		FindObjectOfType<GameSession>().SetHealth(playerHealth);

		//Play sound
		float volume = 0.3f;
		AudioSource.PlayClipAtPoint(hitsounds, Camera.main.transform.position, volume);

		if (playerHealth <= 0) {
			HandleDeath();
			StartCoroutine(FindObjectOfType<GameSession>().StopGameSpace());
			return;
		}

		StartCoroutine(InvincibilityAnimation());
	}

	private IEnumerator InvincibilityAnimation() {
		isInvincible = true;

		float flashDuration = 0.1f;
		int flashCount = 7;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		for (int i = 0; i < flashCount; i++) {
			spriteRenderer.color = Color.clear;
			yield return new WaitForSeconds(flashDuration);
			spriteRenderer.color = Color.white;
			yield return new WaitForSeconds(flashDuration);
		}

		isInvincible = false;
	}

	private void HandleDeath() {
		//Create explosion
		GameObject explosion = Instantiate(
			explosionParticles,
			transform.position,
			Quaternion.identity);
		Destroy(explosion, 1f);

		//Play death sound
		float volume = 0.5f;
		AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, volume);

		isStarted = false;
		isStartAnimation = false;
		//Put player off screen
		Vector2 offScreenPosition = new Vector2(-20f, -20f);
		gameObject.GetComponent<Transform>().position = offScreenPosition;
	}

}
