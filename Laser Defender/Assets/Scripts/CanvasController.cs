using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
	[SerializeField] List<GameObject> canvasElements;
	[SerializeField] float transitionSpeed = 5f;
	[SerializeField] int transitionState = 0;
	[SerializeField] float outOfBoundsRight = 500;
	[SerializeField] float outOfBoundsLeft  = -500;

	public void transitionOut() {
		int placementCounts = 0;

		foreach (GameObject canvasObject in canvasElements) {
			RectTransform objectTransform = canvasObject.GetComponent<RectTransform>();
			//For each canvasElement, move it off screen
			Vector3 outPosition = findOutOfBoundsPosition(canvasObject);
			float movementThisFrame = (transitionSpeed) * Time.deltaTime;
			objectTransform.position = Vector3.MoveTowards(objectTransform.position, outPosition, movementThisFrame);

			if (objectTransform.position == outPosition) {
				//One more object has been placed properly
				placementCounts++;
			}
		}

		//Change transition state once every element is properly placed
		if (placementCounts == canvasElements.Count) { transitionState = 0; }
	}

	public Vector3 findOutOfBoundsPosition(GameObject canvasObject) {
		int fadeOutSide = canvasObject.GetComponent<CanvasElement>().GetFadeOutSide();
		//If the gameObjects x is less than 0, go to the left, if its greater than 0 go to the right
		float xPosition = outOfBoundsLeft;
		if(fadeOutSide == 1) { xPosition = outOfBoundsRight; }

		RectTransform objectTransform = canvasObject.GetComponent<RectTransform>();
		Vector3 outPosition = new Vector3(xPosition, objectTransform.position.y, objectTransform.position.z);

		return outPosition;
	}

	public void transitionIn() {
		int placementCounts = 0;

		foreach (GameObject canvasObject in canvasElements) {
			RectTransform objectTransform = canvasObject.GetComponent<RectTransform>();
			//For each canvasElement, move it off screen
			float inXPosition = canvasObject.GetComponent<CanvasElement>().GetInXPosition();
			Vector3 inPosition = new Vector3(inXPosition, objectTransform.position.y, objectTransform.position.z);
			float movementThisFrame = (transitionSpeed) * Time.deltaTime;
			objectTransform.position = Vector3.MoveTowards(objectTransform.position, inPosition, movementThisFrame);

			if (objectTransform.position == inPosition) {
				//One more object has been placed properly
				placementCounts++;
			}
		}

		//Change transition state once every element is properly placed
		if (placementCounts == canvasElements.Count) { transitionState = 0; }
	}

	// Update is called once per frame
	void Update()
    {
		//STATES:	
		//		0 = no transition
		//		1 = transitionOut
		//		2 = transitionIn
		switch (transitionState) {
			case 0:
				return;
			case 1:
				transitionOut();
				break;
			case 2:
				transitionIn();
				break;
		}
    }

	public void StartTransitionOut() {
		transitionState = 1;
	}

	public void StartTransitionIn() {
		transitionState = 2;
	}

}
