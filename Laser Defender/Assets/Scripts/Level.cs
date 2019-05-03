using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
	[SerializeField] [Range(0f, 5f)] float gameOverDelay = 1f;

	public void LoadStartMenu() {
		SceneManager.LoadScene(0);
	}

	public void LoadGame() {
		SceneManager.LoadScene("Level1");
	}

	public void LoadGameOver() {
		StartCoroutine(WaitAndLoad());
	}

	private IEnumerator WaitAndLoad() {
		yield return new WaitForSeconds(gameOverDelay);
		SceneManager.LoadScene("GameOver");
	}

	public void QuitGame() {
		Application.Quit();
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
