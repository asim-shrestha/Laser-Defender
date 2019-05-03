using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
	[SerializeField] ParticleSystem closeStarfield;
	[SerializeField] ParticleSystem farStarfield;
	[SerializeField] BackgroundScroller backgroundScroller; 
	private ScoreText scoreText;
	private HealthText healthText;
	private int score = 0;
	private int health = 0;
    // Start is called before the first frame update
    void Awake()
    {
		SetUpSingleton();
		scoreText = FindObjectOfType<ScoreText>();
		healthText = FindObjectOfType<HealthText>();
		UpdateScoreText();
    }

	private void SetUpSingleton() {
		if (FindObjectsOfType(GetType()).Length > 1) { Destroy(this.gameObject); }
		else { DontDestroyOnLoad(this.gameObject); }
	}

	public void StartGame() {
		backgroundScroller.StartScrolling();
		closeStarfield.Play();
		farStarfield.Play();

		FindObjectOfType<EnemySpawner>().StartWaves();
	}

	public void StopGame() {
		//Stop the background
		backgroundScroller.StopScrolling();
		closeStarfield.Stop();
		closeStarfield.Clear();
		farStarfield.Stop();
		farStarfield.Clear();


		//Move enemies off screen
		MoveAllEnemies();

		//Stop spawning enemies
		FindObjectOfType<EnemySpawner>().StopWaves();
	}

	private void MoveAllEnemies() {
		var enemies = FindObjectsOfType<Enemy>();
		foreach(Enemy enemy in enemies) {
			enemy.GameOver();
		}
	}
	public void ChangeScore(int scoreValue) {
		if (scoreText == null) { return; }
		score += scoreValue;
		UpdateScoreText();
	}

	public void SetHealth(int health) {
		if (healthText == null) { return; }
		this.health = health;
		UpdateHealthText();
	}

    void UpdateScoreText()
    {
		if (scoreText == null) { return; }
		scoreText.UpdateScore(score);
    }

	void UpdateHealthText() {
		if (scoreText == null) { return; }
		healthText.UpdateHealth(health);
	}


	public void ResetGame() {
		//Destroy all enemies still on screen
		var enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy enemy in enemies) {
			Destroy(enemy.gameObject);
		}

		Destroy(this.gameObject);
	}
}
