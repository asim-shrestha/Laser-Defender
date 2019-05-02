using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private int health = 3;
	private float speed = 3f;
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
		
    }

    // Update is called once per frame
    void Update()
    {
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

	private void OnTriggerEnter2D(Collider2D collision) {
		DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
		this.HandleDamage(damageDealer.GetDamage());
		damageDealer.HandleHit();
	}

	private void HandleDamage(int damage) {
		health-= damage;

		Debug.Log(health);
		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}
}
