using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
	public void UpdateScore(int score) {
		GetComponent<TextMeshProUGUI>().text = score.ToString();
	}
}
