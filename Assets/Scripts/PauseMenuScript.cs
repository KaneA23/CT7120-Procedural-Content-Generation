using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
	private GameObject pauseScreen;

	private bool isPaused;

	private void Awake() {
		pauseScreen = GameObject.Find("PausePanel");
	}

	// Start is called before the first frame update
	void Start() {
		isPaused = false;
		pauseScreen.SetActive(isPaused);

		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			isPaused = !isPaused;
			pauseScreen.SetActive(isPaused);
			Time.timeScale = isPaused ? 0 : 1;

			Cursor.visible = isPaused;
			Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}

	public void OnMenuButtonPressed() {
		SceneManager.LoadScene(0);
	}
}
