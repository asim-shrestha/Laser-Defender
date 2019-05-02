using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
