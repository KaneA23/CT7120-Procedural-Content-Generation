using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Controls the creation of a mesh terrain and allows basic alterations (i.e. layer colouring).
/// Created by: Kane Adams
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MeshTerrainGenerator : MonoBehaviour {
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

	private MeshCollider meshColl;

	public MeshFilter chunkPrefab;

	private ObjectPool<MeshFilter> meshPool;

	private GameObject[,] grid;

	[SerializeField] private int gridX;
	[SerializeField] private int gridZ;

	[SerializeField] private int currentX;
	[SerializeField] private int currentZ;

	[SerializeField] private GameObject player;

	private void Awake() {
		meshFilter = GetComponent<MeshFilter>();
		meshColl = GetComponent<MeshCollider>();

		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Start is called before the first frame update
	void Start() {
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

		xOffset = 0;
		zOffset = 0;

		xOffset = Random.Range(0, 100000);
		zOffset = Random.Range(0, 100000);

		currentX = 0;
		currentZ = 0;



		//CreateMeshShape();
		//CreateNeighbourMesh();
		//UpdateMesh();

		for (int z = -10; z <= 10; z++) {
			for (int x = -10; x <= 10; x++) {
				CreateMeshShapes(x, z);
				UpdateMeshes(x, z);
			}
		}

		foreach (GameObject cell in grid) {
			if (cell == null) {
				continue;
			}

			cell.gameObject.SetActive(false);
			Debug.Log(cell.transform.position.ToString());
		}

		UpdateActiveChunks();
		//InvokeRepeating(nameof(CreateMeshShape), 5f, 5f);
	}

	// Update is called once per frame
	void Update() {
		//if (Input.GetMouseButtonDown(0)) {
		//	if (useRandomOffsets) {
		//		xOffset = Random.Range(0, 10000f);
		//		zOffset = Random.Range(0, 10000f);
		//	}
		//	//CreateMeshShape();
		//	ResetMeshes();
		//}

		if (player.transform.position.x < currentX * 100 && currentX > -9) {
			//xOffset -= 0.01f;
			//CreateMeshShape();
			//ResetMeshes();

			UpdateInactiveChunks();

			currentX--;
			UpdateActiveChunks();
		}
		if (player.transform.position.x > (currentX + 1) * 100 && currentX < 9) {
			//xOffset += 0.01f;
			//CreateMeshShape();
			//ResetMeshes();

			UpdateInactiveChunks();

			currentX++;
			UpdateActiveChunks();
		}
		if (player.transform.position.z < currentZ * 100 && currentZ > -9) {
			//zOffset -= 0.01f;
			//CreateMeshShape();
			//ResetMeshes();

			UpdateInactiveChunks();

			currentZ--;
			UpdateActiveChunks();
		}
		if (player.transform.position.z > (currentZ + 1) * 100 && currentZ < 9) {
			//zOffset += 0.01f;
			//CreateMeshShape();
			//ResetMeshes();
			
			UpdateInactiveChunks();

			currentZ++;
			UpdateActiveChunks();
		}
	}

	void ResetMeshes() {
		GameObject[] oldMeshes = GameObject.FindGameObjectsWithTag("terrain");
		foreach (GameObject mesh in oldMeshes) {
			Destroy(mesh);
		}

		for (int z = -5; z < 5; z++) {
			for (int x = -5; x < 5; x++) {
				CreateMeshShapes(x, z);
				UpdateMeshes(x, z);
			}
		}
	}

	/// <summary>
	/// Creates quad that allows change within y-axis to create different sea levels as well as allowing different colour layers
	/// </summary>
	void CreateMeshShape() {
		if (useRandomOffsets) {
			xOffset = Random.Range(0, 10000f);
			zOffset = Random.Range(0, 10000f);
		}

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

				y = Mathf.PerlinNoise(/*x * 0.3f + xOffset, z * 0.3f + zOffset*/xCoord + xOffset, zCoord + zOffset);

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

				//if (y > 0.3f) {
				//	colours[vertexIndex] = snowColour;
				//} else if (y >= 0.25f) {
				//	colours[vertexIndex] = rockColour;
				//} else {
				//	colours[vertexIndex] = grassColour;
				//}

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

		UpdateMesh();
	}

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

				y = Mathf.PerlinNoise(/*x * 0.3f + xOffset, z * 0.3f + zOffset*/xCoord + xOffset, zCoord + zOffset);

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

				//if (y > 0.3f) {
				//	colours[vertexIndex] = snowColour;
				//} else if (y >= 0.25f) {
				//	colours[vertexIndex] = rockColour;
				//} else {
				//	colours[vertexIndex] = grassColour;
				//}

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

		//UpdateMeshes();
	}

	/// <summary>
	/// Adds all calculated triangles to the mesh
	/// </summary>
	void UpdateMesh() {
		mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.colors = colours;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		//meshColl.sharedMesh = mesh;
	}

	void UpdateMeshes(int a_xPos, int a_zPos) {
		//MeshFilter filter = Instantiate(chunkPrefab, new Vector3(a_xPos * xSize, 0, a_zPos * zSize), Quaternion.identity);
		//MeshFilter filter = meshPool.Get().GetComponent<MeshFilter>();
		MeshFilter filter = meshPool.Get();
		filter.gameObject.transform.position = new Vector3(a_xPos * xSize, 0, a_zPos * zSize);
		filter.gameObject.transform.parent = GameObject.Find("Terrain").transform;

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


	void UpdateInactiveChunks() {
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

	void UpdateActiveChunks() {
		Debug.Log("Current X: " + (currentX + gridX) + ", Current Z: " + (currentZ + gridZ));

		grid[currentX + gridX, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX, currentZ + gridZ + 1].gameObject.SetActive(true);

		grid[currentX + gridX - 1, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX - 1, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX - 1, currentZ + gridZ + 1].gameObject.SetActive(true);

		grid[currentX + gridX + 1, currentZ + gridZ - 1].gameObject.SetActive(true);
		grid[currentX + gridX + 1, currentZ + gridZ].gameObject.SetActive(true);
		grid[currentX + gridX + 1, currentZ + gridZ + 1].gameObject.SetActive(true);
	}

	private void OnDrawGizmos() {
		if (vertices == null) {
			return;
		}

		foreach (Vector3 vertex in vertices) {
			Gizmos.DrawSphere(vertex, 0.1f);
		}
	}
}
