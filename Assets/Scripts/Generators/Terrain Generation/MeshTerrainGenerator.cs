using UnityEngine;

/// <summary>
/// Controls the creation of a mesh terrain and allows basic alterations (i.e. layer colouring).
/// </summary>
public class MeshTerrainGenerator : MonoBehaviour {
	[Header("Referenced Scripts")]
	[SerializeField] private ObjectSpawner spawner;
	private DDOLManager DDOL;

	[Header("Prefabs")]
	[SerializeField] private MeshFilter chunkPrefab;

	// Chunk properties
	private readonly int meshSize = 100;
	private readonly float scale = 2f;
	private readonly float height = 85;

	float[,] noiseMap;

	private Vector2 offsets;

	// Fractal noise values
	private readonly int octaves = 10;
	private readonly float persistance = 0.33f;
	private readonly float lacunarity = 2;

	private Vector3[] vertices;
	private int[] triangles;

	private Color[] colours;
	private Gradient colourGradient;

	private GameObject[,] grid;
	private readonly int gridSize = 11;

	private int currentX;
	private int currentZ;

	private int minX;
	private int minZ;
	private int maxX;
	private int maxZ;

	private GameObject player;

	private void Awake() {
		DDOL = FindObjectOfType<DDOLManager>();

		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Start is called before the first frame update
	private void Start() {
		grid = new GameObject[gridSize * 2, gridSize * 2];

		offsets = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));

		currentX = 0;
		currentZ = 0;

		colourGradient = TerrainColourManager.CreateColourGradient(DDOL.SeaColour, DDOL.GrassColour, DDOL.RockColour, DDOL.SnowColour);

		// Generates 441 chunks (21x21)
		for (int z = -10; z <= 10; z++) {
			for (int x = -10; x <= 10; x++) {
				noiseMap = PerlinNoiseGenerator.GenerateNoise(octaves, persistance, lacunarity, meshSize, scale, offsets, x, z);
				CreateMeshShapes();
				UpdateMeshes(x, z);
			}
		}

		minX = -9;
		minZ = -9;
		maxZ = 9;
		maxX = 9;

		// Generate trees on each mesh and set inactive
		foreach (GameObject cell in grid) {
			if (cell == null) {
				continue;
			}

			spawner.SpawnObjects(cell.transform, ObjectSpawner.EnvProp.TREE, 0.32f * height, 0.5f * height);
			spawner.SpawnObjects(cell.transform, ObjectSpawner.EnvProp.ROCK, 0.32f * height, 0.64f * height);

			cell.SetActive(false);
		}

		UpdateActiveChunks();

		RenderSettings.fog = (player != null);

		Destroy(DDOL.gameObject);   // DDOL not required anymore
	}

	// Update is called once per frame
	private void Update() {
		if (player != null) {
			if (player.transform.position.x < currentX * 100 && currentX > minX) {
				UpdateInactiveChunks();

				currentX--;
				UpdateActiveChunks();
			}
			if (player.transform.position.x > (currentX + 1) * 100 && currentX < maxX) {
				UpdateInactiveChunks();

				currentX++;
				UpdateActiveChunks();
			}
			if (player.transform.position.z < currentZ * 100 && currentZ > minZ) {
				UpdateInactiveChunks();

				currentZ--;
				UpdateActiveChunks();
			}
			if (player.transform.position.z > (currentZ + 1) * 100 && currentZ < maxZ) {
				UpdateInactiveChunks();

				currentZ++;
				UpdateActiveChunks();
			}
		}
	}

	/// <summary>
	/// Creates quad that allows change within y-axis to create different sea levels as well as allowing different colour layers
	/// </summary>
	void CreateMeshShapes() {
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
	}

	/// <summary>
	/// Adds all calculated triangles to the mesh
	/// </summary>
	private void UpdateMeshes(int a_xPos, int a_zPos) {
		MeshFilter filter = Instantiate(chunkPrefab);
		filter.gameObject.transform.position = new Vector3(a_xPos * meshSize, 0, a_zPos * meshSize);
		filter.gameObject.transform.parent = GameObject.Find("Terrain").transform;
		filter.gameObject.layer = LayerMask.NameToLayer("Terrain"); ;

		grid[a_xPos + gridSize, a_zPos + gridSize] = filter.gameObject;

		MeshCollider currentMeshColl = filter.GetComponent<MeshCollider>();

		Mesh currentMesh = filter.mesh;
		currentMesh.Clear();

		currentMesh.vertices = vertices;
		currentMesh.triangles = triangles;
		currentMesh.colors = colours;

		currentMeshColl.sharedMesh = currentMesh;

		currentMesh.RecalculateBounds();
		currentMesh.RecalculateNormals();
		currentMesh.RecalculateTangents();
	}

	#region Chunk System

	/// <summary>
	/// Sets all meshes as inactive
	/// </summary>
	private void UpdateInactiveChunks() {
		grid[currentX + gridSize, currentZ + gridSize - 1].SetActive(false);
		grid[currentX + gridSize, currentZ + gridSize].SetActive(false);
		grid[currentX + gridSize, currentZ + gridSize + 1].SetActive(false);

		grid[currentX + gridSize - 1, currentZ + gridSize - 1].SetActive(false);
		grid[currentX + gridSize - 1, currentZ + gridSize].SetActive(false);
		grid[currentX + gridSize - 1, currentZ + gridSize + 1].SetActive(false);

		grid[currentX + gridSize + 1, currentZ + gridSize - 1].SetActive(false);
		grid[currentX + gridSize + 1, currentZ + gridSize].SetActive(false);
		grid[currentX + gridSize + 1, currentZ + gridSize + 1].SetActive(false);
	}

	/// <summary>
	/// Sets all meshes in a 3x3 grid around player as active
	/// </summary>
	private void UpdateActiveChunks() {
		grid[currentX + gridSize, currentZ + gridSize - 1].SetActive(true);
		grid[currentX + gridSize, currentZ + gridSize].SetActive(true);
		grid[currentX + gridSize, currentZ + gridSize + 1].SetActive(true);

		grid[currentX + gridSize - 1, currentZ + gridSize - 1].SetActive(true);
		grid[currentX + gridSize - 1, currentZ + gridSize].SetActive(true);
		grid[currentX + gridSize - 1, currentZ + gridSize + 1].SetActive(true);

		grid[currentX + gridSize + 1, currentZ + gridSize - 1].SetActive(true);
		grid[currentX + gridSize + 1, currentZ + gridSize].SetActive(true);
		grid[currentX + gridSize + 1, currentZ + gridSize + 1].SetActive(true);
	}

	#endregion Chunk System
}
