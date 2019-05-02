using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
		CheckBounds();
    }

	private void CheckBounds() {
		//Figure out the max Y position
		float offset = 0f;
		float maxYPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + offset;

		//Destroy if offscreen
		if (transform.position.y > maxYPos) {
			Destroy(this.gameObject);
		}
	}
}
