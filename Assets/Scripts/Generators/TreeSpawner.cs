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

	private float treeOffsetY;

	private float radius;
	private readonly int attemptAmount = 10;

	private List<Vector2> points;

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

	/// <summary>
	/// From list of points generated, checks whether object can spawn on created terrain and then spawns it
	/// </summary>
	/// <param name="a_worldPos">Mesh that trees are to be spawned on</param>
	/// <param name="a_objectToSpawn">What environment item is trying to be spawned</param>
	public void SpawnObjects(Transform a_worldPos, EnvProp a_objectToSpawn) {
		radius = Random.Range(10, 15);

		points = PoissonDiscSampler.GeneratePoints(radius, 1000, attemptAmount);

		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(a_worldPos.position.x + point.x, 1000, a_worldPos.position.z + point.y);

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit) && hit.point.y > 32 && hit.point.y < 64) {
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
}
