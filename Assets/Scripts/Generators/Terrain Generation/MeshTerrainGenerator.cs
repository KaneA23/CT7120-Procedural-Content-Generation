using UnityEngine;

/// <summary>
/// Controls the creation of a mesh terrain and allows basic alterations (i.e. layer colouring).
/// </summary>
public class MeshTerrainGenerator : MonoBehaviour {
	private DDOLManager DDOL;

	[Header("Mesh dimensions")]
	private readonly int meshSize = 100;
	private readonly float scale = 2f;
	private readonly float height = 85;

	float[,] noiseMap;

	private Vector2 offsets;

	private Vector3[] vertices;
	private int[] triangles;

	[Header("Terrain Colouring")]
	private Color snowColour;
	private Color rockColour;
	private Color grassColour;
	private Color seaColour;

	private Color[] colours;

	[SerializeField] private MeshFilter chunkPrefab;

	private GameObject[,] grid;

	private readonly int gridSize = 11;

	private int currentX;
	private int currentZ;

	private int minX;
	private int minZ;
	private int maxX;
	private int maxZ;

	private GameObject player;

	private readonly int octaves = 10;
	private readonly float persistance = 0.33f;
	private readonly float lacunarity = 2;

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

		snowColour = DDOL.SnowColour;
		rockColour = DDOL.StoneColour;
		grassColour = DDOL.GrassColour;
		seaColour = DDOL.SeaColour;

		// Generates meshes in 20x20 grid
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

			FindObjectOfType<TreeSpawner>().SpawnObjects(cell.transform, TreeSpawner.EnvProp.TREE, 0.32f * height, 0.5f * height);
			FindObjectOfType<TreeSpawner>().SpawnObjects(cell.transform, TreeSpawner.EnvProp.ROCK, 0.32f * height, 0.64f * height);

			cell.SetActive(false);
		}

		UpdateActiveChunks();

		//CreateMeshShapes(0, 0);
		//UpdateMeshes(0, 0);

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

		int vertexIndex = 0;
		float colourOffset;
		for (int z = 0; z <= meshSize; z++) {
			for (int x = 0; x <= meshSize; x++) {
				vertices[vertexIndex] = new Vector3(x, noiseMap[x, z] * height, z);

				colourOffset = Random.Range(-0.01f, 0.01f);

				// Dependent on how tall the mesh is in the y-axis, different colours are applied
				if (noiseMap[x, z] > 0.7f) {
					colours[vertexIndex] = new Color(snowColour.r + colourOffset, snowColour.g + colourOffset, snowColour.b + colourOffset);
				} else if (noiseMap[x, z] >= 0.5f) {
					colours[vertexIndex] = new Color(rockColour.r + colourOffset, rockColour.g + colourOffset, rockColour.b + colourOffset);
				} else if (noiseMap[x, z] > 0.3f) {
					colours[vertexIndex] = new Color(grassColour.r, grassColour.g + colourOffset, grassColour.b);
				} else {
					colours[vertexIndex] = new Color(seaColour.r, seaColour.g, seaColour.b + colourOffset);
				}

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

		//Vector2[] uvs = new Vector2[vertices.Length];
		//
		//for (int i = 0; i < vertices.Length; i++) {
		//	uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		//}

		currentMesh.vertices = vertices;
		currentMesh.triangles = triangles;
		//currentMesh.uv = uvs;
		currentMesh.colors = colours;

		currentMeshColl.sharedMesh = currentMesh;

		currentMesh.RecalculateBounds();
		currentMesh.RecalculateNormals();
		currentMesh.RecalculateTangents();
	}

	/// <summary>
	/// Sets all meshes as inactive
	/// </summary>
	private void UpdateInactiveChunks() {
		grid[currentX + gridSize, currentZ + gridSize - 1].gameObject.SetActive(false);
		grid[currentX + gridSize, currentZ + gridSize].gameObject.SetActive(false);
		grid[currentX + gridSize, currentZ + gridSize + 1].gameObject.SetActive(false);

		grid[currentX + gridSize - 1, currentZ + gridSize - 1].gameObject.SetActive(false);
		grid[currentX + gridSize - 1, currentZ + gridSize].gameObject.SetActive(false);
		grid[currentX + gridSize - 1, currentZ + gridSize + 1].gameObject.SetActive(false);

		grid[currentX + gridSize + 1, currentZ + gridSize - 1].gameObject.SetActive(false);
		grid[currentX + gridSize + 1, currentZ + gridSize].gameObject.SetActive(false);
		grid[currentX + gridSize + 1, currentZ + gridSize + 1].gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets all meshes in a 3x3 grid around player as active
	/// </summary>
	private void UpdateActiveChunks() {
		grid[currentX + gridSize, currentZ + gridSize - 1].gameObject.SetActive(true);
		grid[currentX + gridSize, currentZ + gridSize].gameObject.SetActive(true);
		grid[currentX + gridSize, currentZ + gridSize + 1].gameObject.SetActive(true);

		grid[currentX + gridSize - 1, currentZ + gridSize - 1].gameObject.SetActive(true);
		grid[currentX + gridSize - 1, currentZ + gridSize].gameObject.SetActive(true);
		grid[currentX + gridSize - 1, currentZ + gridSize + 1].gameObject.SetActive(true);

		grid[currentX + gridSize + 1, currentZ + gridSize - 1].gameObject.SetActive(true);
		grid[currentX + gridSize + 1, currentZ + gridSize].gameObject.SetActive(true);
		grid[currentX + gridSize + 1, currentZ + gridSize + 1].gameObject.SetActive(true);
	}
}
