using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject {

	//Enemy settings
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] int numberOfEnemies = 5;

	//Path settings
	[SerializeField] GameObject pathPrefab;
	[SerializeField] float timeBetweenSpawns = 0.5f;
	[SerializeField] float spawnRandomFactor = 0.2f;
	
	public GameObject GetEnemyPrefab() { return enemyPrefab; }

	public int GetNumberOfEnemies() { return numberOfEnemies; }

	public List<Transform> GetWaypoints() {
		List<Transform> waveWaypoints = new List<Transform>();

		//Get transform of each child in the given path prefab
		foreach (Transform child in pathPrefab.transform) {
			waveWaypoints.Add(child);
		}

		return waveWaypoints;
	}

	public float GetTimeBetweenSpaws() { return timeBetweenSpawns; }

	public float GetSpawnRandomFactor() { return spawnRandomFactor; }

	
}
