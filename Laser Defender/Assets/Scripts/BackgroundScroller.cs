using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	[SerializeField] float scrollSpeed = 0.05f;
	[SerializeField] Material myMaterial;
	[SerializeField] bool isScrolling = false;

	private Vector2 offSet;
    // Start is called before the first frame update
    void Start()
    {
		myMaterial = GetComponent<Renderer>().material;
		offSet = new Vector2(0f, scrollSpeed);
	}

    // Update is called once per frame
    void Update()
    {
		if (isScrolling) { myMaterial.mainTextureOffset += offSet * Time.deltaTime; }
    }

	public void StartScrolling() {
		isScrolling = true;
	}

	public void StopScrolling() {
		isScrolling = false;
	}
}
