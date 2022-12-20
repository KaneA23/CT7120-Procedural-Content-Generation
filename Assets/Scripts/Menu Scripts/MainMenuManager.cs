using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains all scenes accessible within the build.
/// </summary>
public enum SceneIndex {
	MENU,
	TERRAIN,
	NOISE_DEMO,
	POISSON_DEMO,
}

/// <summary>
/// Controls the main menu screen.
/// </summary>
public class MainMenuManager : MonoBehaviour {
	private DDOLManager DDOL;

	[SerializeField] private GameObject loadScreen;

	private void Awake() {
		DDOL = FindObjectOfType<DDOLManager>();
	}

	// Start is called before the first frame update
	private void Start() {
		loadScreen.SetActive(false);
	}

	/// <summary>
	/// If Auto play button pressed, a random seed and the default terrain colours are used
	/// </summary>
	public void OnAutoPlayPressed() {
		DDOL.IsRandomSeed = true;

		DDOL.SnowColour = Color.white;                      // White
		DDOL.RockColour = new Color(0.25f, 0.25f, 0.25f);	// Dark Grey
		DDOL.GrassColour = new Color(0, 0.3f, 0);           // Dark Green
		DDOL.SeaColour = new Color(0, 0, 0.64f);            // Blue
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnPlayPressed() {
		loadScreen.SetActive(true);

		SceneManager.LoadSceneAsync((int)SceneIndex.TERRAIN);
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnNoiseDemoPressed() {
		loadScreen.SetActive(true);
		Destroy(DDOL.gameObject);

		SceneManager.LoadSceneAsync((int)SceneIndex.NOISE_DEMO);
	}

	/// <summary>
	/// Begins to load next scene behind load screen
	/// </summary>
	public void OnPoissonDemoPressed() {
		loadScreen.SetActive(true);
		Destroy(DDOL.gameObject);

		SceneManager.LoadSceneAsync((int)SceneIndex.POISSON_DEMO);
	}

	/// <summary>
	/// Leaves game
	/// </summary>
	public void OnExitPressed() {
		Application.Quit();
	}
}
