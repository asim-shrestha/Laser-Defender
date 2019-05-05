﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int health;
	[SerializeField] float speed = 5f;
	[SerializeField] int scoreValue = 100;

	[SerializeField] float shotcounter;
	[SerializeField] float mintimeBetweenShots = 0.2f;
	[SerializeField] float maxtimeBetweenShots = 3f;
	[SerializeField] GameObject laser;
	float laserSpeed = 5f;

	private WaveConfig waveConfig;
	[SerializeField] GameObject explosionParticles;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip hitSound;
	[SerializeField] bool isGameOver = false;
	[SerializeField] float outofBoundsLeft = -5f;
	[SerializeField] float outofBoundsRight = 5f;

	List<Transform> wayPoints = new List<Transform>();
	private int waypointIndex = 0;

	public void SetWaveConfig(WaveConfig waveConfig) {
		wayPoints = waveConfig.GetWaypoints();

		//Place enemy on the first waypoint
		transform.position = wayPoints[waypointIndex].position;
		waypointIndex++;
	}

	// Start is called before the first frame update
	void Start()
    {
		shotcounter = Random.Range(mintimeBetweenShots, maxtimeBetweenShots);
    }

    // Update is called once per frame
    void Update() {
		Move();
		CountdownShotCounter();
		
	}
	
	public void GameOver() {
		wayPoints.Add(this.transform);
		isGameOver = true;
	}

	private void Move() {
		//Check if we have reached the end of the path
		if (waypointIndex <= wayPoints.Count - 1) {
			//Move towards next waypoint
			var targetPosition = wayPoints[waypointIndex].position;

			//If the game is over, move towards left of screen
			if (isGameOver) {
				float outOfBounds = outofBoundsLeft;

				if(transform.position.x > 0) { outOfBounds = outofBoundsRight; }

				targetPosition = new Vector2(outOfBounds, transform.position.y);
			}

			var movementhThisFrame = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementhThisFrame);

			//Check if waypoint reached
			if (transform.position == targetPosition) {
				waypointIndex++;
			}
		}

		//No path specified so do nothing
		else if (wayPoints.Count == 0) {
			return;
		}

		//End of path reached
		else {
			Destroy(this.gameObject);
		}
	}

	private void CountdownShotCounter() {
		shotcounter -= Time.deltaTime;

		//If the shot counter is expired and the game hasn't ended
		if (shotcounter <= 0f && isGameOver == false) {
			StartCoroutine(Fire());
			shotcounter = Random.Range(mintimeBetweenShots, maxtimeBetweenShots);
		}
	}

	private IEnumerator Fire() {
		GameObject newLaser = Instantiate(
			laser,
			transform.position,
			Quaternion.identity
			);
		newLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);

		//Ship recoil
		float recoilOffset = 0.08f;
		float newYPos = transform.position.y + recoilOffset;    //Recoil position
		transform.position = new Vector2(transform.position.x, newYPos);
		float recoilDelay = 0.05f;
		yield return new WaitForSeconds(recoilDelay);
		newYPos = transform.position.y - recoilOffset;          //Reset position
		transform.position = new Vector2(transform.position.x, newYPos);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

		//If damageDealer is null, return
		if (!damageDealer) { return; }

		this.HandleDamage(damageDealer.GetDamage());
		damageDealer.HandleHit();
	}

	private void HandleDamage(int damage) {
		health-= damage;

		//Play sound
		float volume = 0.3f;
		AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, volume);

		if (health <= 0) {
			HandleDeath();
			Destroy(this.gameObject);
		}

		else {
			StartCoroutine(HitAnimation());
		}
	}

	private IEnumerator HitAnimation() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		float flashTimer = 0.075f;
		Color original = spriteRenderer.color;

		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(flashTimer);
		spriteRenderer.color = original;
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

		//Update score
		FindObjectOfType<GameSession>().ChangeScore(scoreValue);
	}
}
