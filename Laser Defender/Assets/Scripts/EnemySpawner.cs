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
			//if(enemyCount > 0) {
				//waveIndex--;
				//continue;
			//}


			WaveConfig currentWave = waveConfigs[waveIndex];
			yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
		}

		yield return new WaitForSeconds(0f);
	}

		private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave) {
		int numberOfEnemies = currentWave.GetNumberOfEnemies();

		//Spawn enemies and set their wave configuration
		for (int i = 0; i < numberOfEnemies; i++) {
			GameObject newEnemy = Instantiate(
				currentWave.GetEnemyPrefab(),
				currentWave.GetWaypoints()[0].position,
				Quaternion.identity);

			if (isStarted == false) { Destroy(newEnemy.gameObject); }
			else { enemyCount++; }

			newEnemy.GetComponent<Enemy>().SetWaveConfig(currentWave);
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
