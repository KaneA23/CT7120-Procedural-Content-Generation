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

	// Update is called once per frame
	void Update() {
		Debug.Log("random colour currently: " + DDOL.IsRandomColours.ToString());
		Debug.Log("random seed currently: " + DDOL.IsRandomSeed.ToString());
	}

	/// <summary>
	/// If Auto play button pressed, a random seed and the default terrain colours are used
	/// </summary>
	public void OnAutoPlayPressed() {
		DDOL.IsRandomSeed = true;

		DDOL.IsRandomColours = false;

		DDOL.SnowColour = new Color(0.85f, 0.85f, 0.85f);   // White
		DDOL.StoneColour = new Color(0.35f, 0.35f, 0.35f);  // Dark Grey
		DDOL.GrassColour = new Color(0, 0.3f, 0.1f);        // Dark Green
		DDOL.SeaColour = new Color(0, 0, 0.4f);             // Blue
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

	/// <summary>
	/// While scene is being loaded, show load screen
	/// </summary>
	/// <returns>Enters coroutine every frame until level is loaded</returns>
	private IEnumerator ShowLoadingProgress() {
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);

		//loadbar.value = 0;

		while (!loadOperation.isDone) {
			float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);

			//loadbar.value = progress;
			Debug.Log(progress);

			yield return null;
		}
	}
}
