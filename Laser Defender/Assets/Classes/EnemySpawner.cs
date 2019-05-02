using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] float timeBetweenWaves;

	int waveIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
		//Go through all waves and initiate them
		while (waveIndex < waveConfigs.Count) {
			Debug.Log(waveIndex);
			WaveConfig currentWave = waveConfigs[waveIndex];
			StartCoroutine(SpawnAllEnemiesInWave(currentWave));
			waveIndex++;
		}

    }

	private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave) {
		int numberOfEnemies = currentWave.GetNumberOfEnemies();

		//Spawn enemies and set their waypoints
		for (int i = 0; i < numberOfEnemies; i++) {
			Instantiate(
				currentWave.GetEnemyPrefab(),
				currentWave.GetWaypoints()[0].position,
				Quaternion.identity);

			yield return new WaitForSeconds(currentWave.GetTimeBetweenSpaws());
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
