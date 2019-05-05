using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasElement : MonoBehaviour
{
	[SerializeField] float inXPosition = 0f;
	[SerializeField] [Range(0,1)] int fadeOutSide = 1;
	//0 is to the left, 1 is to the right

	public float GetInXPosition() {
		return inXPosition;
	}

	public int GetFadeOutSide() {
		return fadeOutSide;
	}
}
