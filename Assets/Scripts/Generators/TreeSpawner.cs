using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of trees and other gameobjects within a meshed area.
/// </summary>
public class TreeSpawner : MonoBehaviour {
	[SerializeField] private GameObject[] treePrefabs;

	private GameObject currentTree;
	private RaycastHit hit;
	private Vector3 raycastOrigin;

	[SerializeField] private LayerMask terrainLayer;
	private int treeAmount;
	private float treeOffsetY;


	public float radius = 5;
	public int attemptAmount = 10;

	List<Vector2> points;

	/// <summary>
	/// Types of objects the player can spawn
	/// </summary>
	public enum EnvProp {
		TREE,
		ROCK,
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void SpawnObjects(Transform a_worldPos, EnvProp a_objectToSpawn) {
		radius = Random.Range(10, 15);

		points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);

		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(a_worldPos.position.x + point.x, 100, a_worldPos.position.z + point.y);

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit) && hit.point.y > 15 && hit.point.y < 35) {
				currentTree = Instantiate(treePrefabs[(int)a_objectToSpawn]);
				currentTree.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentTree.transform.parent = a_worldPos;
				currentTree.transform.position = hit.point;

				if (currentTree.transform.localScale.y > 1) {
					treeOffsetY = currentTree.transform.position.y + 1/*+ (currentTree.transform.localScale.y / 2)*/;
					currentTree.transform.localRotation = Quaternion.Euler(0, currentTree.transform.localRotation.y, 0);
				} else {
					treeOffsetY = currentTree.transform.position.y + (currentTree.transform.localScale.y / 2);
				}

				currentTree.transform.localPosition = new Vector3(raycastOrigin.x - a_worldPos.position.x, treeOffsetY, raycastOrigin.z - a_worldPos.position.z);
			}
		}
	}

	/// <summary>
	/// Spawns trees around
	/// </summary>
	/// <param name="a_worldPos">Mesh that trees are to be spawned on</param>
	public void SpawnTrees(Transform a_worldPos) {
		treeAmount = Random.Range(50, 150);

		for (int i = 0; i < treeAmount; i++) {
			raycastOrigin = new Vector3(a_worldPos.position.x + Random.Range(0, 100), a_worldPos.position.y + 100, a_worldPos.position.z + Random.Range(0, 100));

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 150, terrainLayer) && hit.point.y > 15 && hit.point.y < 35) {
				//currentTree = Instantiate(treePrefab);
				currentTree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)]);
				currentTree.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentTree.transform.parent = a_worldPos;
				currentTree.transform.position = hit.point;

				if (currentTree.transform.localScale.y > 1) {
					treeOffsetY = currentTree.transform.position.y + 1/*+ (currentTree.transform.localScale.y / 2)*/;
					currentTree.transform.localRotation = Quaternion.Euler(Random.Range(-5, 5), currentTree.transform.localRotation.y, Random.Range(-5, 5));
				} else {
					treeOffsetY = currentTree.transform.position.y + (currentTree.transform.localScale.y / 2);
				}

				currentTree.transform.localPosition = new Vector3(raycastOrigin.x - a_worldPos.position.x, treeOffsetY, raycastOrigin.z - a_worldPos.position.z);
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Environmental")) {
			Destroy(gameObject);
		}
	}
}
