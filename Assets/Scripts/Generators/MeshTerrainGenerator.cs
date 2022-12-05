using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Controls the creation of a mesh terrain and allows basic alterations (i.e. layer colouring).
/// </summary>
public class MeshTerrainGenerator : MonoBehaviour {
	private DDOLManager DDOL;

	private Mesh mesh;
	private MeshFilter meshFilter;

	[Header("Mesh dimensions")]
	[SerializeField] private int xSize;
	[SerializeField] private int zSize;
	[SerializeField] private float scale;
	[SerializeField] private int height;

	// Each chunk is 5x5 in perlin noise offsets and 100x100 in mesh position
	[Space(10)]
	[SerializeField] private float xOffset;
	[SerializeField] private float zOffset;
	[SerializeField] private bool useRandomOffsets;

	private Vector3[] vertices;
	private int[] triangles;

	[Header("Terrain Shading")]
	[SerializeField] private Color snowColour = Color.white;
	[SerializeField] private Color rockColour = Color.grey;
	[SerializeField] private Color grassColour = new Color(0, 0.25f, 0.1f); // Dark Green
	[SerializeField] private Color seaColour = new Color(0, 0, 0.4f);       // Blue

	private Color[] colours;

	[SerializeField] private MeshFilter chunkPrefab;

	private ObjectPool<MeshFilter> meshPool;

	private GameObject[,] grid;

	[SerializeField] private int gridX;
	[SerializeField] private int gridZ;

	private int currentX;
	private int currentZ;

	private int minX;
	private int minZ;
	private int maxX;
	private int maxZ;

	private GameObject player;

	private void Awake() {
		DDOL = FindObjectOfType<DDOLManager>();

		meshFilter = GetComponent<MeshFilter>();

		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Start is called before the first frame update
	private void Start() {
		meshPool = new ObjectPool<MeshFilter>(() => {
			return Instantiate(chunkPrefab);
		}, meshObj => {
			meshObj.gameObject.SetActive(true);
		}, meshObj => {
			meshObj.gameObject.SetActive(false);
		}, meshObj => {
			Destroy(meshObj.gameObject);
		}, false, 9, 100);

		grid = new GameObject[gridX * 2, gridZ * 2];

		mesh = new Mesh();
		meshFilter.mesh = mesh;

		xOffset = Random.Range(0, 100000);
		zOffset = Random.Range(0, 100000);

		currentX = 0;
		currentZ = 0;

		if (DDOL.IsRandomColours) {
			snowColour = Random.ColorHSV();
			rockColour = Random.ColorHSV();
			grassColour = Random.ColorHSV();
			seaColour = Random.ColorHSV();

			snowColour.a = 1;
			rockColour.a = 1;
			grassColour.a = 1;
			seaColour.a = 1;
		} else {
			snowColour = DDOL.SnowColour;
			rockColour = DDOL.StoneColour;
			grassColour = DDOL.GrassColour;
			seaColour = DDOL.SeaColour;
		}

		// Generates meshes in 10x10 grid
		for (int z = -10; z <= 10; z++) {
			for (int x = -10; x <= 10; x++) {
				CreateMeshShapes(x, z);
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

			FindObjectOfType<TreeSpawner>().SpawnTrees(cell.transform);

			cell.gameObject.SetActive(false);
		}

		UpdateActiveChunks();

		RenderSettings.fog = (player != null);
	}

	// Update is called once per frame
	private void Update() {
		//if (Input.GetMouseButtonDown(0)) {
		//	if (useRandomOffsets) {
		//		xOffset = Random.Range(0, 10000f);
		//		zOffset = Random.Range(0, 10000f);
		//	}
		//	//CreateMeshShape();
		//	ResetMeshes();
		//}

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
	void CreateMeshShapes(int a_xOffset, int a_zOffset) {
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		colours = new Color[vertices.Length];

		// Uses perlin noise to get the height of the terrain based on the x and z axis + offset
		int vertexIndex = 0;
		float xCoord;
		float zCoord;
		float y;
		for (int z = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++) {
				xCoord = (float)x / xSize * scale;
				zCoord = (float)z / zSize * scale;

				xCoord += (5 * a_xOffset);
				zCoord += (5 * a_zOffset);

				y = Mathf.PerlinNoise(xCoord + xOffset, zCoord + zOffset);

				vertices[vertexIndex] = new Vector3(x, y * height, z);

				// Dependent on how tall the mesh is in the y-axis, different colours are applied
				if (y > 0.85f) {
					colours[vertexIndex] = snowColour;
				} else if (y > 0.65f) {
					colours[vertexIndex] = rockColour;
				} else if (y > 0.25f) {
					colours[vertexIndex] = grassColour; //new Color(grassColour.r, grassColour.g +Random.Range(-0.1f, 0.1f), grassColour.b);
				} else {
					colours[vertexIndex] = seaColour;
				}

				vertexIndex++;
			}
		}

		// Generates 2 triangles between 4 vertices to create quads
		triangles = new int[xSize * zSize * 6];
		int vert = 0;
		int tris = 0;
		for (int z = 0; z < zSize; z++) {
			for (int x = 0; x < xSize; x++) {
				// Triangle 1
				triangles[tris + 0] = vert;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;

				// Triangle 2
				triangles[tris + 3] = triangles[tris + 2];
				triangles[tris + 4] = triangles[tris + 1];
				triangles[tris + 5] = vert + xSize + 2;

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
		MeshFilter filter = meshPool.Get();
		filter.gameObject.transform.position = new Vector3(a_xPos * xSize, 0, a_zPos * zSize);
		filter.gameObject.transform.parent = GameObject.Find("Terrain").transform;
		filter.gameObject.layer = LayerMask.NameToLayer("Terrain"); ;

		grid[a_xPos + gridX, a_zPos + gridZ] = filter.gameObject;

		MeshCollider currentMeshColl = filter.GetComponent<MeshCollider>();

		Mesh currentMesh = filter.mesh;
		currentMesh.Clear();

		currentMesh.vertices = vertices;
		currentMesh.triangles = triangles;
		currentMesh.colors = colours;

		currentMeshColl.sharedMesh = currentMesh;

		currentMesh.RecalculateBounds();
		currentMesh.RecalculateNormals();
	}

	/// <summary>
	/// Sets all meshes as inactive
	/// </summary>
	private void UpdateInactiveChunks() {
		grid[currentX + gridX, currentZ + gridZ - 1].gameObject.SetActive(false);
		grid[currentX + gridX, currentZ + gridZ].gameObject.SetActive(false);
		grid[currentX + gridX, currentZ + gridZ + 1].gameObject.SetActive(false);

		grid[currentX + gridX - 1, currentZ + gridZ - 1].gameObject.SetActive(false);
		grid[currentX + gridX - 1, currentZ + gridZ].gameObject.SetActive(false);
		grid[currentX + gridX - 1, currentZ + gridZ + 1].gameObject.SetActive(false);

		grid[currentX + gridX + 1, currentZ + gridZ - 1].gameObject.SetActive(false);
		grid[currentX + gridX + 1, currentZ + gridZ].gameObject.SetActive(false);
		grid[currentX + gridX + 1, currentZ + gridZ + 1].gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets all meshes in a 3x3 grid around player as active
	/// </summary>
	private void UpdateActiveChunks() {
		grid[currentX + gridX, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX, currentZ + gridZ + 1].gameObject.SetActive(true);

		grid[currentX + gridX - 1, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX - 1, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX - 1, currentZ + gridZ + 1].gameObject.SetActive(true);

		grid[currentX + gridX + 1, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX + 1, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX + 1, currentZ + gridZ + 1].gameObject.SetActive(true);

		Debug.Log("Current cell: (" + (currentX + gridX) + ", " + (currentZ + gridZ) + ")");

		//if ((currentX + gridX - 2) > 0 && grid[(currentX + gridX - 2), currentZ + gridZ] == null) {
		//	Debug.Log("Ungenerated cell: (" + (currentX + gridX - 2) + ", " + (currentZ + gridZ) + ")");
		//
		//	CreateMeshShapes((currentX - 2), currentZ);
		//	UpdateMeshes((currentX - 2), currentZ);
		//
		//	minX--;
		//
		//	//grid[(currentX + gridX - 1), currentZ + gridZ].SetActive(false);
		//
		//	Debug.Log("Created new mesh at: (" + (currentX + gridX - 2) + ", " + (currentZ + gridZ) + ")");
		//}
		//if ((currentZ + gridZ - 2) > 0 && grid[currentX + gridX, (currentZ + gridZ - 2)] == null) {
		//	Debug.Log("Ungenerated cell: (" + (currentX + gridX) + ", " + (currentZ + gridZ - 2) + ")");
		//
		//	CreateMeshShapes(currentX, (currentZ - 2));
		//	UpdateMeshes(currentX, (currentZ - 2));
		//
		//	minZ--;
		//
		//	//grid[currentX + gridX, currentZ + gridZ - 1].SetActive(false);
		//
		//	Debug.Log("Created new mesh at: (" + (currentX + gridX) + ", " + (currentZ + gridZ - 2) + ")");
		//}
		//if ((currentX + gridX + 2) < ((gridX * 2) - 1) && grid[(currentX + gridX + 2), currentZ + gridZ] == null) {
		//	Debug.Log("Ungenerated cell: (" + (currentX + gridX + 2) + ", " + (currentZ + gridZ) + ")");
		//
		//	CreateMeshShapes((currentX + 2), currentZ);
		//	UpdateMeshes((currentX + 2), currentZ);
		//
		//	maxX++;
		//
		//	//grid[(currentX + gridX - 1), currentZ + gridZ].SetActive(false);
		//
		//	Debug.Log("Created new mesh at: (" + (currentX + gridX + 2) + ", " + (currentZ + gridZ) + ")");
		//}
		//if ((currentZ + gridZ + 2) < ((gridZ * 2) - 1) && grid[currentX + gridX, (currentZ + gridZ + 2)] == null) {
		//	Debug.Log("Ungenerated cell: (" + (currentX + gridX) + ", " + (currentZ + gridZ + 2) + ")");
		//
		//	CreateMeshShapes(currentX, (currentZ + 2));
		//	UpdateMeshes(currentX, (currentZ + 2));
		//
		//	maxZ++;
		//
		//	//grid[currentX + gridX, currentZ + gridZ - 1].SetActive(false);
		//
		//	Debug.Log("Created new mesh at: (" + (currentX + gridX) + ", " + (currentZ + gridZ + 2) + ")");
		//}
	}
}
