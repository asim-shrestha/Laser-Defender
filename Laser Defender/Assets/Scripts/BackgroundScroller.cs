using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	[SerializeField] float scrollSpeed = 0.026f;
	[SerializeField] float changeInScrollOffset = 0.002f;
	[SerializeField] Material myMaterial;
	[SerializeField] bool isScrolling = false;

	private Vector2 offSet;
    // Start is called before the first frame update
    void Start()
    {
		myMaterial = GetComponent<Renderer>().material;
		offSet = new Vector2(0f, 0f);
	}

	// Update is called once per frame
	void Update() {
		//If we're scrolling, increase scroll offset until the desired speed is reached
		if (isScrolling) {
			if (offSet.y < scrollSpeed) {
				offSet = new Vector2(0f, offSet.y += changeInScrollOffset);
			}
		}

		//Decrease scroll offset until the screen no longer scrolls
		else {
			if (offSet.y > 0) {
				offSet = new Vector2(0f, offSet.y -= changeInScrollOffset);
			}
		}

		myMaterial.mainTextureOffset += offSet * Time.deltaTime;
	}

	public void StartScrolling() {
		isScrolling = true;
	}

	public void StopScrolling() {
		isScrolling = false;
	}
}
