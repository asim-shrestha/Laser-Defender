using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private int health = 3;
	[SerializeField] private float speed = 3f;

	[SerializeField] float shotcounter;
	[SerializeField] float mintimeBetweenShots = 0.2f;
	[SerializeField] float maxtimeBetweenShots = 3f;
	[SerializeField] GameObject laser;
	float laserSpeed = 5f;

	List<Transform> wayPoints = new List<Transform>();
	private int waypointIndex = 0;

	public void SetWaveConfig(WaveConfig waveConfig) {
		//Configure enemy
		health = waveConfig.GetEnemyHealth();
		speed = waveConfig.GetEnemySpeed();
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

	private void Move() {
		//Check if we have reached the end of the path
		if (waypointIndex <= wayPoints.Count - 1) {
			//Move towards next waypoint
			var targetPosition = wayPoints[waypointIndex].position;
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
		if (shotcounter <= 0f) {
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
		transform.position = new Vector2(transform.position.x, transform.position.y + recoilOffset);
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

		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}
}
