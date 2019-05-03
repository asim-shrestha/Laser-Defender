using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
	TextMeshProUGUI scoreText;
	private int score;
    // Start is called before the first frame update
    void Awake()
    {
		SetUpSingleton();
		scoreText = FindObjectOfType<TextMeshProUGUI>();
		UpdateScoreText();
    }

	private void SetUpSingleton() {
		if (FindObjectsOfType(GetType()).Length > 1) { Destroy(this.gameObject); }
		else { DontDestroyOnLoad(this.gameObject); }
	}

    void UpdateScoreText()
    {
		scoreText.text = score.ToString();
    }

	public void AddScore(int scoreValue) {
		score += scoreValue;
		UpdateScoreText();
	}

	public void ResetGame() {
		Destroy(this.gameObject);
	}
}
