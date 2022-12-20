using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the main menu screen.
/// </summary>
public class MainMenuManager : MonoBehaviour {
	private DDOLManager DDOL;

	[SerializeField] private GameObject loadScreen;
	//[SerializeField] private Slider loadbar;

	private void Awake() {
		DDOL = FindObjectOfType<DDOLManager>();
	}

	// Start is called before the first frame update
	private void Start() {
		//loadbar.value = 0;
		loadScreen.SetActive(false);
	}

	/// <summary>
	/// If Auto play button pressed, a random seed and the default terrain colours are used
	/// </summary>
	public void OnAutoPlayPressed() {
		DDOL.IsRandomSeed = true;

		DDOL.SnowColour = Color.white;   // White
		DDOL.StoneColour = new Color(0.25f, 0.25f, 0.25f);  // Dark Grey
		DDOL.GrassColour = new Color(0, 0.3f, 0);        // Dark Green
		DDOL.SeaColour = new Color(0, 0, 0.64f);             // Blue
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnPlayPressed() {
		loadScreen.SetActive(true);

		SceneManager.LoadSceneAsync(1);

		//StartCoroutine(ShowLoadingProgress(1));
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnNoiseDemoPressed() {
		loadScreen.SetActive(true);
		Destroy(DDOL.gameObject);

		SceneManager.LoadSceneAsync(2);

		//StartCoroutine(ShowLoadingProgress(2));
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnPoissonDemoPressed() {
		loadScreen.SetActive(true);
		Destroy(DDOL.gameObject);

		SceneManager.LoadSceneAsync(3);

		//StartCoroutine(ShowLoadingProgress(3));
	}

	/// <summary>
	/// Leaves game
	/// </summary>
	public void OnExitPressed() {
		Application.Quit();
	}

	///// <summary>
	///// While scene is being loaded, show load screen
	///// </summary>
	///// <returns>Enters coroutine every frame until level is loaded</returns>
	//private IEnumerator ShowLoadingProgress(int a_sceneIndex) {
	//	AsyncOperation loadOperation = SceneManager.LoadSceneAsync(a_sceneIndex);
	//
	//	//loadbar.value = 0;
	//
	//	while (!loadOperation.isDone) {
	//		float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
	//
	//		loadbar.value = progress;
	//
	//		yield return null;
	//	}
	//}
}
