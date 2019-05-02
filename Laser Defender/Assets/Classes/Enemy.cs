using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int health = 3;
	[SerializeField] float enemySpeed = 5f;
	[SerializeField] List<Transform> wayPoints;
	private int wayPointIndex = 0;

	// Start is called before the first frame update
	void Start()
    {
		//Start at the first waypoint
		transform.position = wayPoints[wayPointIndex].position;
		wayPointIndex++;
    }

    // Update is called once per frame
    void Update()
    {
		//Check if we have reached the end of the path
		if (wayPointIndex <= wayPoints.Count - 1) {
			//Move towards next waypoint
			var targetPosition = wayPoints[wayPointIndex].position;
			var movementhThisFrame = enemySpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementhThisFrame);

			//Check if waypoint reached
			if(transform.position == targetPosition) {
				wayPointIndex++;
			}
		}

		else {
			Destroy(this.gameObject);
		}

    }

	private void OnTriggerEnter2D(Collider2D collision) {
		Destroy(collision.gameObject);
		HandleHit();
	}

	private void HandleHit() {
		health--;

		Debug.Log(health);
		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}
}
