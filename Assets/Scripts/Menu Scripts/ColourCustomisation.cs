using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the customisation of the terrain colours.
/// </summary>
public class ColourCustomisation : MonoBehaviour {
	private DDOLManager DDOL;

	[Header("Colour Elements")]
	[SerializeField] private Image colourPreview;

	[Space(5)]
	[SerializeField] private Slider redSlider;
	[SerializeField] private Slider greenSlider;
	[SerializeField] private Slider blueSlider;

	[Space(5)]
	private Color defaultColour;
	[SerializeField] private Color chosenColour;

	private bool useRandomColours;

	public bool UseRandomColours {
		get {
			return useRandomColours;
		}

		set {
			useRandomColours = value;
		}
	}

	private void Awake() {
		DDOL = FindObjectOfType<DDOLManager>();
	}

	// Start is called before the first frame update
	private void Start() {
		useRandomColours = false;
		DDOL.IsRandomSeed = false;

		redSlider.value = chosenColour.r;
		greenSlider.value = chosenColour.g;
		blueSlider.value = chosenColour.b;

		colourPreview.color = chosenColour;
		defaultColour = chosenColour;
	}

	#region Sliders

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnRedSliderChange() {
		chosenColour.r = redSlider.value;
		colourPreview.color = chosenColour;
	}

	/// <summary>
	/// Changes the amount of green in terrain
	/// </summary>
	public void OnGreenSliderChange() {
		chosenColour.g = greenSlider.value;
		colourPreview.color = chosenColour;
	}

	/// <summary>
	/// Changes the amount of vlue in terrain
	/// </summary>
	public void OnBlueSliderChange() {
		chosenColour.b = blueSlider.value;
		colourPreview.color = chosenColour;
	}

	#endregion Sliders

	/// <summary>
	/// Changes colours for terrain to default ones
	/// </summary>
	public void OnDefaultButtonPressed() {
		chosenColour = defaultColour;

		redSlider.value = chosenColour.r;
		greenSlider.value = chosenColour.g;
		blueSlider.value = chosenColour.b;
	}

	/// <summary>
	/// Stores the chosen colour of terrain segment in DDOL before changing scene
	/// </summary>
	private void OnDestroy() {
		if (useRandomColours) {
			chosenColour = Random.ColorHSV();
			chosenColour.a = 1;
		}

		// Uses the name of gameobject to determine which segment colour is being set
		switch (gameObject.name) {
			case "Snow":
				DDOL.SnowColour = chosenColour;
				break;

			case "Rock":
				DDOL.RockColour = chosenColour;
				break;

			case "Grass":
				DDOL.GrassColour = chosenColour;
				break;

			case "Sea":
				DDOL.SeaColour = chosenColour;
				break;
		}
	}
}
