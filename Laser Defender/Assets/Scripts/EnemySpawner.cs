using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] List<WaveConfig> pointConfigs;
	[SerializeField] float timeBetweenWaves;
	[SerializeField] bool isLooping = true;
	[SerializeField] bool isStarted = false;

	[SerializeField] int enemyCount = 0;

	int waveIndex = 0;
    // Start is called before the first frame update
	//Start is made to be a coroutine
    IEnumerator Start()
    {
		do {
			yield return StartCoroutine(SpawnAllWaves());
		}
		while (isLooping);
    }

	private IEnumerator SpawnAllWaves() {
		int waveConfigsLen = waveConfigs.Count;
		enemyCount = 0;

		//Go through all waves and initiate them
		for (waveIndex = 0; waveIndex < waveConfigsLen; waveIndex++) {
			if (isStarted == false) { break; }

			//Check if the last wave has finished yet
			if(enemyCount > 0) {
				waveIndex--;
				yield return new WaitForSeconds(timeBetweenWaves);
				continue;
			}


			WaveConfig currentWave = waveConfigs[waveIndex];
			yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));

			WaveConfig currentPoints = pointConfigs[waveIndex];
			yield return StartCoroutine(SpawnAllEnemiesInPoints(currentPoints));
		}

		//If the for loop is not reached
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator SpawnAllEnemiesInPoints(WaveConfig currentPoints) {
		int numberOfEnemies = currentPoints.GetNumberOfEnemies();
		Vector3 startingPosition = new Vector3(0f, 7f, 0f);

		//Spawn enemies and set their wave configuration
		for (int i = 0; i < numberOfEnemies - 1; i++) {
			GameObject newEnemy = Instantiate(
				currentPoints.GetEnemyPrefab(),
				startingPosition,
				Quaternion.identity);

			if (isStarted == false) { Destroy(newEnemy.gameObject); }
			else { enemyCount++; }

			List<Transform> wayPoints = new List<Transform>();
			wayPoints.Add(newEnemy.transform);
			wayPoints.Add(currentPoints.GetWaypoints()[i]);

			newEnemy.GetComponent<Enemy>().SetWaveConfig(wayPoints);
			newEnemy.GetComponent<Enemy>().StayAtPathEnd();
			yield return new WaitForSeconds(currentPoints.GetTimeBetweenSpaws() / 3f);
		}
	}

	private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave) {
		int numberOfEnemies = currentWave.GetNumberOfEnemies();
		numberOfEnemies = Math.Abs(numberOfEnemies);

		//Spawn enemies and set their wave configuration
		for (int i = 0; i < numberOfEnemies; i++) {
			GameObject newEnemy = Instantiate(
				currentWave.GetEnemyPrefab(),
				currentWave.GetWaypoints()[0].position,
				Quaternion.identity);

			if (isStarted == false) { Destroy(newEnemy.gameObject); }
			else { enemyCount++; }

			newEnemy.GetComponent<Enemy>().SetWaveConfig(currentWave.GetWaypoints());
			yield return new WaitForSeconds(currentWave.GetTimeBetweenSpaws());
		}
	}

	public void RemoveEnemy() {
		if (enemyCount >= 0) { enemyCount--; }
	}

	public void StartWaves() {
		isStarted = true;
	}

	public void StopWaves() {
		isStarted = false;
	}

}
