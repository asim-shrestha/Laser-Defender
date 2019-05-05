﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
	[Header("Menu and Background")]
	[SerializeField] Canvas menuCanvas;
	[SerializeField] GameObject gameSpace;
	[SerializeField] ParticleSystem closeStarfield;
	[SerializeField] ParticleSystem farStarfield;
	[SerializeField] BackgroundScroller backgroundScroller;

	[Header("Game Space")]
	[SerializeField] Canvas gameCanvas;
	[SerializeField] EnemySpawner enemySpawner;
	[SerializeField] Player player;

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

		menuCanvas.enabled = true;
		gameCanvas.enabled = false;
    }

	private void SetUpSingleton() {
		if (FindObjectsOfType(GetType()).Length > 1) { Destroy(this.gameObject); }
		else { DontDestroyOnLoad(this.gameObject); }
	}

	public void StartGame() {
		score = 0;
		health = 0;
		StartGameSpace();
		menuCanvas.enabled = false;
		gameCanvas.enabled = true;
		backgroundScroller.StartScrolling();
		closeStarfield.Play();
		farStarfield.Play();

		UpdateScoreText();
		UpdateHealthText();
		FindObjectOfType<EnemySpawner>().StartWaves();
	}

	private void StartGameSpace() {
	}

	private void EndGameSpace() {
	}

	public void StopGame() {
		//Stop the background
		backgroundScroller.StopScrolling();
		closeStarfield.Stop();
		farStarfield.Stop();

		//Move enemies off screen
		MoveAllEnemies();

		//Stop spawning enemies
		FindObjectOfType<EnemySpawner>().StopWaves();

		//Activate proper menu
		gameCanvas.enabled = false;
		menuCanvas.enabled = true;

		ResetGame();
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
			//Destroy(enemy.gameObject);
		}

		//Reset gameSession
		score = 0;
		health = 0;
	}
}
