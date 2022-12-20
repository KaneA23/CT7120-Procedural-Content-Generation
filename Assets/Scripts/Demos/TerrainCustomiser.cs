using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the changes to the terrain demo dependent on user inputs to UI.
/// </summary>
public class TerrainCustomiser : MonoBehaviour {
	private TerrainDemoScript TDS;

	[Header("Perrlin Noise Properties")]
	[SerializeField] private Slider scaleSlider;
	[SerializeField] private Slider heightSlider;
	[Space(5)]
	[SerializeField] private Slider offsetXSlider;
	[SerializeField] private Slider offsetZSlider;

	[Header("Fractal Noise Properties")]
	[SerializeField] private Slider octaveSlider;
	[SerializeField] private Slider persistanceSlider;
	[SerializeField] private Slider lacunaritySlider;

	[Header("Colour Properties")]
	[SerializeField] private Slider snowR;
	[SerializeField] private Slider snowG;
	[SerializeField] private Slider snowB;

	[Space(5)]
	[SerializeField] private Slider rockR;
	[SerializeField] private Slider rockG;
	[SerializeField] private Slider rockB;

	[Space(5)]
	[SerializeField] private Slider grassR;
	[SerializeField] private Slider grassG;
	[SerializeField] private Slider grassB;

	[Space(5)]
	[SerializeField] private Slider seaR;
	[SerializeField] private Slider seaG;
	[SerializeField] private Slider seaB;

	private void Awake() {
		TDS = GetComponent<TerrainDemoScript>();
	}

	// Start is called before the first frame update
	private void Start() {
		scaleSlider.value = TDS.Scale;
		heightSlider.value = TDS.Height;

		offsetXSlider.value = TDS.OffsetX;
		offsetZSlider.value = TDS.OffsetZ;


		octaveSlider.value = TDS.Octaves;
		persistanceSlider.value = TDS.Persistance;
		lacunaritySlider.value = TDS.Lacunarity;


		snowR.value = TDS.SnowColour.r;
		snowG.value = TDS.SnowColour.g;
		snowB.value = TDS.SnowColour.b;

		rockR.value = TDS.RockColour.r;
		rockG.value = TDS.RockColour.g;
		rockB.value = TDS.RockColour.b;

		grassR.value = TDS.GrassColour.r;
		grassG.value = TDS.GrassColour.g;
		grassB.value = TDS.GrassColour.b;

		seaR.value = TDS.SeaColour.r;
		seaG.value = TDS.SeaColour.g;
		seaB.value = TDS.SeaColour.b;
	}

	#region Mesh Sliders

	/// <summary>
	/// Changes how zoomed into the perlin noise is
	/// </summary>
	public void ChangeScale() {
		TDS.Scale = scaleSlider.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes how tall the mesh can be
	/// </summary>
	public void ChangeHeight() {
		TDS.Height = heightSlider.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the position of perlin noise on X-axis
	/// </summary>
	public void ChangeXOffset() {
		TDS.OffsetX = (int)offsetXSlider.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the position of perlin noise on Z-axis
	/// </summary>
	public void ChangeZOffset() {
		TDS.OffsetZ = (int)offsetZSlider.value;
		TDS.CreateMeshShape();
	}

	#endregion Mesh Sliders

	#region Fractal Noise Sliders

	/// <summary>
	/// Changes how many times the perlin noise is ran
	/// </summary>
	public void ChangeOctaves() {
		TDS.Octaves = (int)octaveSlider.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes how much impact on noisemap each octave has
	/// </summary>
	public void ChangePersistance() {
		TDS.Persistance = persistanceSlider.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes how much detail octaves add
	/// </summary>
	public void ChangeLacunarity() {
		TDS.Lacunarity = lacunaritySlider.value;
		TDS.CreateMeshShape();
	}

	#endregion Fractal Noise Sliders

	#region Colour Sliders

	/// <summary>
	/// Changes the amount of red in snow region
	/// </summary>
	public void OnRedSnowChange() {
		TDS.SnowColour = new Color(snowR.value, TDS.SnowColour.g, TDS.SnowColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of green in snow region
	/// </summary>
	public void OnGreenSnowChange() {
		TDS.SnowColour = new Color(TDS.SnowColour.r, snowG.value, TDS.SnowColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of blue in snow region
	/// </summary>
	public void OnBlueSnowChange() {
		TDS.SnowColour = new Color(TDS.SnowColour.r, TDS.SnowColour.g, snowB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in rock region
	/// </summary>
	public void OnRedRockChange() {
		TDS.RockColour = new Color(rockR.value, TDS.RockColour.g, TDS.RockColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of green in rock region
	/// </summary>
	public void OnGreenRockChange() {
		TDS.RockColour = new Color(TDS.RockColour.r, rockG.value, TDS.RockColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of blue in rock region
	/// </summary>
	public void OnBlueRockChange() {
		TDS.RockColour = new Color(TDS.RockColour.r, TDS.RockColour.g, rockB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in grass region
	/// </summary>
	public void OnRedGrassChange() {
		TDS.GrassColour = new Color(grassR.value, TDS.GrassColour.g, TDS.GrassColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of blue in grass region
	/// </summary>
	public void OnGreenGrassChange() {
		TDS.GrassColour = new Color(TDS.GrassColour.r, grassG.value, TDS.GrassColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of blue in grass region
	/// </summary>
	public void OnBlueGrassChange() {
		TDS.GrassColour = new Color(TDS.GrassColour.r, TDS.GrassColour.g, grassB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in sea region
	/// </summary>
	public void OnRedSeaChange() {
		TDS.SeaColour = new Color(seaR.value, TDS.SeaColour.g, TDS.SeaColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of green in sea region
	/// </summary>
	public void OnGreenSeaChange() {
		TDS.SeaColour = new Color(TDS.SeaColour.r, seaG.value, TDS.SeaColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of blue in sea region
	/// </summary>
	public void OnBlueSeaChange() {
		TDS.SeaColour = new Color(TDS.SeaColour.r, TDS.SeaColour.g, seaB.value);
		TDS.CreateMeshShape();
	}

	#endregion Colour Sliders
}
