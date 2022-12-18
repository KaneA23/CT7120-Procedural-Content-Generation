using UnityEngine;
using UnityEngine.UI;

public class TerrainCustomiser : MonoBehaviour {
	private TerrainDemoScript TDS;

	[Header("Perrlin Noise Properties")]
	[SerializeField] private Slider scale;
	[SerializeField] private Slider height;
	[Space(5)]
	[SerializeField] private Slider offsetX;
	[SerializeField] private Slider offsetZ;

	[Header("Fractal Noise Properties")]
	[SerializeField] private Slider octaves;
	[SerializeField] private Slider persistance;
	[SerializeField] private Slider lacunarity;

	[Header("Colour Properties")]
	[SerializeField] private Slider snowR;
	[SerializeField] private Slider snowG;
	[SerializeField] private Slider snowB;
	[Space(5)]
	[SerializeField] private Slider stoneR;
	[SerializeField] private Slider stoneG;
	[SerializeField] private Slider stoneB;
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
		scale.value = TDS.Scale;
		height.value = TDS.Height;

		offsetX.value = TDS.OffsetX;
		offsetZ.value = TDS.OffsetZ;


		octaves.value = TDS.Octaves;
		persistance.value = TDS.Persistance;
		lacunarity.value = TDS.Lacunarity;


		snowR.value = TDS.SnowColour.r;
		snowG.value = TDS.SnowColour.g;
		snowB.value = TDS.SnowColour.b;

		stoneR.value = TDS.RockColour.r;
		stoneG.value = TDS.RockColour.g;
		stoneB.value = TDS.RockColour.b;

		grassR.value = TDS.GrassColour.r;
		grassG.value = TDS.GrassColour.g;
		grassB.value = TDS.GrassColour.b;

		seaR.value = TDS.SeaColour.r;
		seaG.value = TDS.SeaColour.g;
		seaB.value = TDS.SeaColour.b;
	}

	public void ChangeScale() {
		TDS.Scale = scale.value;
		TDS.CreateMeshShape();
	}

	public void ChangeHeight() {
		TDS.Height = height.value;
		TDS.CreateMeshShape();
	}

	public void ChangeXOffset() {
		TDS.OffsetX = (int)offsetX.value;
		TDS.CreateMeshShape();
	}

	public void ChangeZOffset() {
		TDS.OffsetZ = (int)offsetZ.value;
		TDS.CreateMeshShape();
	}

	public void ChangeOctaves() {
		TDS.Octaves = (int)octaves.value;
		TDS.CreateMeshShape();
	}

	public void ChangePersistance() {
		TDS.Persistance = persistance.value;
		TDS.CreateMeshShape();
	}

	public void ChangeLacunarity() {
		TDS.Lacunarity = lacunarity.value;
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnRedSnowChange() {
		TDS.SnowColour = new Color(snowR.value, TDS.SnowColour.g, TDS.SnowColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnGreenSnowChange() {
		TDS.SnowColour = new Color(TDS.SnowColour.r, snowG.value, TDS.SnowColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnBlueSnowChange() {
		TDS.SnowColour = new Color(TDS.SnowColour.r, TDS.SnowColour.g, snowB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnRedRockChange() {
		TDS.RockColour = new Color(stoneR.value, TDS.RockColour.g, TDS.RockColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnGreenRockChange() {
		TDS.RockColour = new Color(TDS.RockColour.r, stoneG.value, TDS.RockColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnBlueRockChange() {
		TDS.RockColour = new Color(TDS.RockColour.r, TDS.RockColour.g, stoneB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnRedGrassChange() {
		TDS.GrassColour = new Color(grassR.value, TDS.GrassColour.g, TDS.GrassColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnGreenGrassChange() {
		TDS.GrassColour = new Color(TDS.GrassColour.r, grassG.value, TDS.GrassColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnBlueGrassChange() {
		TDS.GrassColour = new Color(TDS.GrassColour.r, TDS.GrassColour.g, grassB.value);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnRedSeaChange() {
		TDS.SeaColour = new Color(seaR.value, TDS.SeaColour.g, TDS.SeaColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnGreenSeaChange() {
		TDS.SeaColour = new Color(TDS.SeaColour.r, seaG.value, TDS.SeaColour.b);
		TDS.CreateMeshShape();
	}

	/// <summary>
	/// Changes the amount of red in terrain
	/// </summary>
	public void OnBlueSeaChange() {
		TDS.SeaColour = new Color(TDS.SeaColour.r, TDS.SeaColour.g, seaB.value);
		TDS.CreateMeshShape();
	}
}