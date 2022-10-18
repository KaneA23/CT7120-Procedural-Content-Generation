using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshTerrainGenerator : MonoBehaviour {
	private LehmerPRNG PRNG;

	private Mesh mesh;
	private MeshFilter meshFilter;

	[SerializeField] private int xSize;
	[SerializeField] private int zSize;

	[SerializeField] private float xOffset;
	[SerializeField] private float zOffset;

	private Vector3[] vertices;
	private int[] triangles;

	private Color[] colours;

	private void Awake() {
		meshFilter = GetComponent<MeshFilter>();
	}

	// Start is called before the first frame update
	void Start() {
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		PRNG = new LehmerPRNG();

		xOffset = PRNG.GenerateNumber(10000);
		zOffset = PRNG.GenerateNumber(10000);

		CreateMeshShape();
		UpdateMesh();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			xOffset = Random.Range(0, 10000f); //PRNG.GenerateNumber(10000);
			zOffset = Random.Range(0, 10000f); //PRNG.GenerateNumber(10000);

			CreateMeshShape();
			UpdateMesh();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	void CreateMeshShape() {
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		colours = new Color[vertices.Length];

		int vertexIndex = 0;
		for (int z = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++) {
				float y = Mathf.PerlinNoise(x * 0.3f + xOffset, z * 0.3f + zOffset) * 2f;

				vertices[vertexIndex] = new Vector3(x, y, z);

				Color snowColour = Color.white;
				Color rockColour = Color.grey;
				Color grassColour = new Color(0, 0.4f, 0.1f);
				Color seaColour = new Color(0, 0, 0.4f);

				if (y >= (0.8f * 2f)) {
					colours[vertexIndex] = snowColour;
				} else if (y >= (0.6f * 2f)) {
					colours[vertexIndex] = rockColour;
				} else if (y >= (0.4f * 2f)) {
					colours[vertexIndex] = grassColour;
				} else {
					colours[vertexIndex] = seaColour;
				}

				vertexIndex++;
			}
		}

		triangles = new int[xSize * zSize * 6];

		int vert = 0;
		int tris = 0;

		// Generates 2 triangles between 4 points to create quads
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
