using UnityEngine;

/// <summary>
/// Controls the mesh generation for terrain demo.
/// Mesh can be altered by user through changing mesh shape, amount of detail and colours used.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainDemoScript : MonoBehaviour {
	private Mesh mesh;
	private MeshFilter meshFilter;

	private readonly int meshSize = 100;

	private float scale;
	private float height;

	private int offsetX;
	private int offsetZ;

	// Fractal noise properties
	private int octaves;
	private float persistance;
	private float lacunarity;

	// Colour properties
	private Color snowColour = Color.white;
	private Color rockColour = Color.grey;
	private Color grassColour = new Color(0, 0.25f, 0.1f); // Dark Green
	private Color seaColour = new Color(0, 0, 0.4f);       // Blue

	private Gradient colourGradient = new Gradient();

	#region GetterSetters

	public float Scale {
		get {
			return scale;
		}

		set {
			scale = value;
		}
	}

	public float Height {
		get {
			return height;
		}

		set {
			height = value;
		}
	}

	public int OffsetX {
		get {
			return offsetX;
		}

		set {
			offsetX = value;
		}
	}

	public int OffsetZ {
		get {
			return offsetZ;
		}

		set {
			offsetZ = value;
		}
	}

	public int Octaves {
		get {
			return octaves;
		}

		set {
			octaves = value;
		}
	}

	public float Persistance {
		get {
			return persistance;
		}

		set {
			persistance = value;
		}
	}

	public float Lacunarity {
		get {
			return lacunarity;
		}

		set {
			lacunarity = value;
		}
	}

	public Color SnowColour {
		get {
			return snowColour;
		}

		set {
			snowColour = value;
		}
	}

	public Color RockColour {
		get {
			return rockColour;
		}

		set {
			rockColour = value;
		}
	}

	public Color GrassColour {
		get {
			return grassColour;
		}

		set {
			grassColour = value;
		}
	}
	public Color SeaColour {
		get {
			return seaColour;
		}

		set {
			seaColour = value;
		}
	}

	#endregion GetterSetters

	private float[,] noiseMap;

	private Vector3[] vertices;
	private int[] triangles;

	private Color[] colours;

	private void Awake() {
		meshFilter = GetComponent<MeshFilter>();
	}

	// Start is called before the first frame update
	private void Start() {
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		scale = 2f;
		height = 75;

		octaves = 10;
		persistance = 0.33f;
		lacunarity = 2f;

		offsetX = Random.Range(0, 100000);
		offsetZ = Random.Range(0, 100000);

		snowColour = Color.white;
		rockColour = new Color(0.25f, 0.25f, 0.25f);
		grassColour = new Color(0, 0.3f, 0);
		seaColour = new Color(0, 0, 0.64f);

		CreateMeshShape();
	}

	/// <summary>
	/// Creates the data for the mesh shape and colours depending on noisemap generated
	/// </summary>
	public void CreateMeshShape() {
		colourGradient = TerrainColourManager.CreateColourGradient(SeaColour, GrassColour, RockColour, SnowColour);

		noiseMap = PerlinNoiseGenerator.GenerateNoise(octaves, persistance, lacunarity, meshSize, scale, new Vector2(offsetX, offsetZ));

		vertices = new Vector3[(meshSize + 1) * (meshSize + 1)];
		colours = new Color[vertices.Length];

		// Uses the perlin noise to create points on mesh
		int vertexIndex = 0;
		for (int z = 0; z <= meshSize; z++) {
			for (int x = 0; x <= meshSize; x++) {
				vertices[vertexIndex] = new Vector3(x, noiseMap[x, z] * height, z);

				//sets up which colour to use where gradient time is the y-axis
				colours[vertexIndex] = colourGradient.Evaluate(noiseMap[x, z]);

				vertexIndex++;
			}
		}

		// Generates 2 triangles between 4 vertices to create quads
		triangles = new int[meshSize * meshSize * 6];
		int vert = 0;
		int tris = 0;
		for (int z = 0; z < meshSize; z++) {
			for (int x = 0; x < meshSize; x++) {
				// Triangle 1
				triangles[tris + 0] = vert;
				triangles[tris + 1] = vert + meshSize + 1;
				triangles[tris + 2] = vert + 1;

				// Triangle 2
				triangles[tris + 3] = triangles[tris + 2];
				triangles[tris + 4] = triangles[tris + 1];
				triangles[tris + 5] = vert + meshSize + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}

		UpdateMesh();
	}

	/// <summary>
	/// Adds all calculated triangles to the mesh
	/// </summary>
	private void UpdateMesh() {
		mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.colors = colours;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
	}
}
