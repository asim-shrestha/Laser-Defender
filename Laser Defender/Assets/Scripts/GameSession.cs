using System.Collections;
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

	[SerializeField] private int score = 0;
	[SerializeField] private int health = 0;
    // Start is called before the first frame update
    void Awake()
    {
		SetUpSingleton();
		gameCanvas.GetComponent<CanvasController>().StartTransitionOut();
    }

	private void SetUpSingleton() {
		if (FindObjectsOfType(GetType()).Length > 1) { Destroy(this.gameObject); }
		else { DontDestroyOnLoad(this.gameObject); }
	}

	public void StartGame() {
		//Enable background
		backgroundScroller.StartScrolling();
		closeStarfield.Play();
		farStarfield.Play();

		StartCoroutine(StartGameSpace());
		
	}

	private IEnumerator StartGameSpace() {
		float waitTimer = 2f;

		//Wait for the background scrolling to start
		//Move menu text
		menuCanvas.GetComponent<CanvasController>().StartTransitionOut();
		yield return new WaitForSeconds(waitTimer / 2.5f);
		gameCanvas.GetComponent<CanvasController>().StartTransitionIn();

		//Enable player
		ResetGame();
		player.StartPlayer();
		UpdateScoreTexts();
		UpdateHealthText();



		yield return new WaitForSeconds(waitTimer);

		//Start waves
		FindObjectOfType<EnemySpawner>().StartWaves();
	}

	public IEnumerator StopGameSpace() {
		//Stop the background
		backgroundScroller.StopScrolling();
		closeStarfield.Stop();
		farStarfield.Stop();

		//Move enemies off screen
		MoveAllEnemies();

		//Stop spawning enemies
		FindObjectOfType<EnemySpawner>().StopWaves();

		//Activate and disable canvases 
		yield return new WaitForSeconds(1f);
		gameCanvas.GetComponent<CanvasController>().StartTransitionOut();
		yield return new WaitForSeconds(0.5f);
		menuCanvas.GetComponent<CanvasController>().StartTransitionIn();
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

	private void MoveAllEnemies() {
		var enemies = FindObjectsOfType<Enemy>();
		foreach(Enemy enemy in enemies) {
			enemy.GameOver();
		}
	}
	public void ChangeScore(int scoreValue) {
		score += scoreValue;
		UpdateScoreTexts();
	}

    void UpdateScoreTexts()
    {
		ScoreText[] scoreTexts = FindObjectsOfType<ScoreText>();
		if (scoreTexts.Length == 0) { return; }
		foreach (ScoreText scoreText in scoreTexts) {
			scoreText.UpdateScore(score);
		}
    }

	public void SetHealth(int health) {
		this.health = health;
		UpdateHealthText();
	}

	void UpdateHealthText() {
		HealthText healthText = FindObjectOfType<HealthText>();
		if(healthText == null) { return; }
		healthText.UpdateHealth(health);
	}

}
