using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the creation of a mesh terrain and allows basic alterations (i.e. layer colouring).
/// Created by: Kane Adams
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshTerrainGenerator : MonoBehaviour {
	private Mesh mesh;
	private MeshFilter meshFilter;

	[Header("Mesh dimensions")]
	[SerializeField] private int xSize;
	[SerializeField] private int zSize;

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

	private void Awake() {
		meshFilter = GetComponent<MeshFilter>();
	}

	// Start is called before the first frame update
	void Start() {
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		xOffset = Random.Range(0, 10000f);
		zOffset = Random.Range(0, 10000f);

		CreateMeshShape();
		//UpdateMesh();

		InvokeRepeating(nameof(CreateMeshShape), 5f, 5f);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			CreateMeshShape();
			//UpdateMesh();
		}

		if (useRandomOffsets) {
			xOffset = Random.Range(0, 10000f);
			zOffset = Random.Range(0, 10000f);
		}


	}

	/// <summary>
	/// Creates quad that allows change within y-axis to create different sea levels as well as allowing different colour layers
	/// </summary>
	void CreateMeshShape() {
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		colours = new Color[vertices.Length];

		// Uses perlin noise to get the height of the terrain based on the x and z axis + offset
		int vertexIndex = 0;
		for (int z = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++) {
				float y = Mathf.PerlinNoise(x * 0.3f + xOffset, z * 0.3f + zOffset) * 2f;

				vertices[vertexIndex] = new Vector3(x, y, z);

				// Dependent on how tall the mesh is in the y-axis, different colours are applied
				if (y > (0.85f * 2f)) {
					colours[vertexIndex] = snowColour;
				} else if (y > (0.65f * 2f)) {
					colours[vertexIndex] = rockColour;
				} else if (y >= (0.25f * 2f)) {
					colours[vertexIndex] = grassColour;
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

		UpdateMesh();
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
