using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	[SerializeField] int damage = 1;
	[SerializeField] GameObject hitParticles;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update() {
		CheckBounds();
	}

	private void CheckBounds() {
		//Figure out the max Y position
		float offset = 1f;
		float minXPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - offset;
		float maxXPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + offset;
		float minYPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - offset;
		float maxYPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + offset;

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
