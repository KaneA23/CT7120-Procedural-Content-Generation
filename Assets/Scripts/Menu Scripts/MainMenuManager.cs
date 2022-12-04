using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the main menu screen.
/// Created by: Kane Adams
/// </summary>
public class MainMenuManager : MonoBehaviour {
	[SerializeField] private GameObject loadScreen;
	//[SerializeField] private Slider loadbar;

	// Start is called before the first frame update
	private void Start() {
		//loadbar.value = 0;
		loadScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnPlayPressed() {
		loadScreen.SetActive(true);

		StartCoroutine(ShowLoadingProgress());
	}

	/// <summary>
	/// Leaves game
	/// </summary>
	public void OnExitPressed() {
		Debug.Log("Exited game");
		Application.Quit();
	}

	private IEnumerator ShowLoadingProgress() {
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);

		while (!loadOperation.isDone) {
			float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);

			//loadbar.value = progress;
			Debug.Log(progress);

			yield return null;
		}
	}
}
