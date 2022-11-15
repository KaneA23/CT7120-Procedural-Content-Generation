using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void Awake() {
		meshFilter = GetComponent<MeshFilter>();
		meshColl = GetComponent<MeshCollider>();
	}

	// Start is called before the first frame update
	void Start() {
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		xOffset = 0;
		zOffset = 0;

		//CreateMeshShape();
		//CreateNeighbourMesh();
		//UpdateMesh();

		for (int z = 0; z < 5; z++) {
			for (int x = 0; x < 5; x++) {
				CreateMeshShapes(x, z);
				UpdateMeshes(x, z);
			}
		}


		//InvokeRepeating(nameof(CreateMeshShape), 5f, 5f);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (useRandomOffsets) {
				xOffset = Random.Range(0, 10000f);
				zOffset = Random.Range(0, 10000f);
			}
			//CreateMeshShape();
			ResetMeshes();
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			xOffset -= 0.01f;
			//CreateMeshShape();
			ResetMeshes();
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			xOffset += 0.01f;
			//CreateMeshShape();
			ResetMeshes();
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			zOffset -= 0.01f;
			//CreateMeshShape();
			ResetMeshes();
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			zOffset += 0.01f;
			//CreateMeshShape();
			ResetMeshes();
		}
	}

	void ResetMeshes() {
		GameObject[] oldMeshes = GameObject.FindGameObjectsWithTag("terrain");
		foreach (GameObject mesh in oldMeshes) {
			Destroy(mesh);
		}

		for (int z = 0; z < 5; z++) {
			for (int x = 0; x < 5; x++) {
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
		Debug.Log("Creating many meshes");

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
		MeshFilter filter = Instantiate(chunkPrefab, new Vector3(a_xPos * xSize, 0, a_zPos * zSize), Quaternion.identity);

		MeshCollider currentMeshColl = filter.GetComponent<MeshCollider>();

		Mesh currentMesh = filter.mesh;
		currentMesh.Clear();

		currentMesh.vertices = vertices;
		currentMesh.triangles = triangles;
		currentMesh.colors = colours;

		//currentMeshColl.sharedMesh = currentMesh;

		currentMesh.RecalculateBounds();
		currentMesh.RecalculateNormals();
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
