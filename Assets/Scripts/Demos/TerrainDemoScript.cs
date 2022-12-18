using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainDemoScript : MonoBehaviour {
	private Mesh mesh;
	private MeshFilter meshFilter;

	private readonly int meshSize = 100;

	private float scale;
	private float height;

	private int offsetX;
	private int offsetZ;

	private int octaves;
	private float persistance;
	private float lacunarity;

	private Color snowColour = Color.white;
	private Color rockColour = Color.grey;
	private Color grassColour = new Color(0, 0.25f, 0.1f); // Dark Green
	private Color seaColour = new Color(0, 0, 0.4f);       // Blue

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
	}public Color SeaColour {
		get {
			return seaColour;
		}

		set {
			seaColour = value;
		}
	}

	private float[,] noiseMap;

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

		scale = 2f;
		height = 75;

		octaves = 10;
		persistance = 0.33f;
		lacunarity = 2f;

		offsetX = Random.Range(0, 100000);
		offsetZ = Random.Range(0, 100000);

		snowColour = Color.white;
		rockColour = Color.grey;
		grassColour = new Color(0, 0.25f, 0.1f);
		seaColour = new Color(0, 0, 0.4f);

		CreateMeshShape();
	}

	public void CreateMeshShape() {
		noiseMap = PerlinNoiseGenerator.GenerateNoise(octaves, persistance, lacunarity, meshSize, scale, new Vector2(offsetX, offsetZ));

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

		UpdateMesh();
	}

	/// <summary>
	/// Adds all calculated triangles to the mesh
	/// </summary>
	private void UpdateMesh() {
		mesh.Clear();

		//Vector2[] uvs = new Vector2[vertices.Length];
		//
		//for (int i = 0; i < vertices.Length; i++) {
		//	uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		//}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		//currentMesh.uv = uvs;
		mesh.colors = colours;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
	}
}
