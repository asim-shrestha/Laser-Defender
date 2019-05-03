using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	[SerializeField] int damage = 1;
	[SerializeField] AudioClip laserSound;
	[SerializeField] GameObject hitParticles;

	private float minXPos;
	private float maxXPos;
	private float minYPos;
	private float maxYPos;

	// Start is called before the first frame update
	void Start()
    {
		AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, 0.07f);

		//Figure out the edges of the screen
		float offset = 0.5f;
		minXPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - offset;
		maxXPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + offset;
		minYPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - offset;
		maxYPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + offset;
	}

	// Update is called once per frame
	void Update() {
		CheckBounds();
	}

	private void CheckBounds() {
		//Destroy if offcamera
		if (transform.position.x < minXPos ||
			transform.position.x > maxXPos ||
			transform.position.y < minYPos ||
			transform.position.y > maxYPos
			) {
			Destroy(this.gameObject);
			return;
		}
	}

	public int GetDamage() {
		return damage;
	}

	public void HandleHit() {
		CreateHitParticles();
		Destroy(this.gameObject);
	}

	public void CreateHitParticles() {
		GameObject explosion = Instantiate(
			hitParticles,
			transform.position,
			Quaternion.identity);
		Destroy(explosion, 1f);
	}
}
