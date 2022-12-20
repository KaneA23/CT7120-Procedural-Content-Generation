using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls when to pause the game and show menu screen dependent on player input.
/// </summary>
public class PauseMenuScript : MonoBehaviour {
	[SerializeField] private GameObject pauseScreen;

	private bool isPaused;

	// Start is called before the first frame update
	private void Start() {
		isPaused = false;
		pauseScreen.SetActive(isPaused);

		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	private void Update() {
		// Toggles pause menu when player presses pause(/resume) button
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
			isPaused = !isPaused;
			pauseScreen.SetActive(isPaused);
			Time.timeScale = isPaused ? 0 : 1;
		}
	}

	/// <summary>
	/// Returns to main menu scene
	/// </summary>
	public void OnMenuButtonPressed() {
		SceneManager.LoadSceneAsync((int)SceneIndex.MENU);
	}
}
