using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] float timeBetweenWaves;
	[SerializeField] bool isLooping = true;
	[SerializeField] bool isSpawning = true;

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
			WaveConfig currentWave = waveConfigs[waveIndex];
			yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
		}
	}

		private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave) {
		int numberOfEnemies = currentWave.GetNumberOfEnemies();

		//Spawn enemies and set their wave configuration
		for (int i = 0; i < numberOfEnemies; i++) {
			GameObject newEnemy = Instantiate(
				currentWave.GetEnemyPrefab(),
				currentWave.GetWaypoints()[0].position,
				Quaternion.identity);
			newEnemy.GetComponent<Enemy>().SetWaveConfig(currentWave);

			if(isSpawning == false) {
				Destroy(newEnemy.gameObject);
			}

			yield return new WaitForSeconds(currentWave.GetTimeBetweenSpaws());
		}
	}

	public void StartWaves() {
		isSpawning = true;
	}

	public void StopWaves() {
		isSpawning = false;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
