using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Enemy Configuration")]
	[SerializeField] bool isStarted = false;
	[SerializeField] int health;
	[SerializeField] float speed = 5f;
	[SerializeField] int scoreValue = 100;
	[SerializeField] bool loopWave = false;
	[SerializeField] bool stayAtPathEnd = false;

	[Header("Laser Configuration")]
	[SerializeField] float shotcounter;
	[SerializeField] float mintimeBetweenShots = 0.2f;
	[SerializeField] float maxtimeBetweenShots = 3f;
	[SerializeField] GameObject laser;
	[SerializeField] float laserSpeed = 5f;

	[Header("Other")]
	[SerializeField] GameObject explosionParticles;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip hitSound;
	[SerializeField] float outofBoundsLeft = -5f;
	[SerializeField] float outofBoundsRight = 5f;

	private WaveConfig waveConfig;
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
		stayAtPathEnd = false;
		loopWave = false;
		isStarted = false;
	}

	private void Move() {
		//Check if we have reached the end of the path
		if (waypointIndex <= wayPoints.Count - 1) {
			//Move towards next waypoint
			var targetPosition = wayPoints[waypointIndex].position;

			if (!isStarted) {
				//If the game is over, move towards left of screen
				//Check if the ship should move to the left or right
				float outOfBounds = outofBoundsLeft;
				if (transform.position.x > 0) { outOfBounds = outofBoundsRight; }
				targetPosition = new Vector2(outOfBounds, transform.position.y);
			}

			var movementThisFrame = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

			//Check if waypoint reached
			if (transform.position == targetPosition) {
				waypointIndex++;
			}
		}

		//No path specified so do nothing
		else if (wayPoints.Count == 0) {
			return;
		}

		//End of unlooping path reached
		else if (stayAtPathEnd) {
			wayPoints.Clear();
			waypointIndex = 0;
			stayAtPathEnd = false;
		}

		//Loop the path
		else if (loopWave && (wayPoints.Count > 0)) {
			transform.position = wayPoints[0].position;
			waypointIndex = 1;
		}


		else {
			Destroy(this.gameObject);
			FindObjectOfType<EnemySpawner>().RemoveEnemy();
		}
	}

	private void CountdownShotCounter() {
		shotcounter -= Time.deltaTime;

		//If the shot counter is expired and the game has started
		if (shotcounter <= 0f && isStarted == true) {
			StartCoroutine(Fire());
			shotcounter = Random.Range(mintimeBetweenShots, maxtimeBetweenShots);
		}
	}

	private IEnumerator Fire() {
		//Create a laser that aims down
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
			FindObjectOfType<EnemySpawner>().RemoveEnemy();
		}

		else {
			StartCoroutine(FlashRed());
		}
	}

	private IEnumerator FlashRed() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		float flashTimer = 0.075f;

		Color hitColor = new Color(1f, 0.25f, 0.25f);	//A red colour
		spriteRenderer.color = hitColor;
		yield return new WaitForSeconds(flashTimer);
		spriteRenderer.color = Color.white;
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
