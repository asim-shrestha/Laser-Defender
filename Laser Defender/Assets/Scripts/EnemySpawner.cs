using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] float timeBetweenWaves;
	[SerializeField] bool isLooping = true;
	[SerializeField] bool isStarted = false;

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

		//Go through all waves and initiate them
		for (waveIndex = 0; waveIndex < waveConfigsLen; waveIndex++) {
			if (isStarted == false) { break; }

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

			newEnemy.GetComponent<Enemy>().SetWaveConfig(currentWave);
			yield return new WaitForSeconds(currentWave.GetTimeBetweenSpaws());
		}
	}

	public void StartWaves() {
		isStarted = true;
	}

	public void StopWaves() {
		isStarted = false;
	}

}
