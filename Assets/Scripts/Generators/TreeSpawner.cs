using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of trees and other gameobjects within a meshed area.
/// Created by: Kane Adams
/// </summary>
public class TreeSpawner : MonoBehaviour {
	[SerializeField] private GameObject treePrefab;

	private GameObject currentTree;
	private RaycastHit hit;
	private Vector3 raycastOrigin;

	[SerializeField] private LayerMask terrainLayer;
	private int treeAmount;
	private float treeOffsetY;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Spawns trees around
	/// </summary>
	/// <param name="a_worldPos">Mesh that trees are to be spawned on</param>
	public void SpawnTrees(Transform a_worldPos) {
		//Vector3 raycastOrigin = new Vector3(a_worldPos.position.x, a_worldPos.position.y + 10, a_worldPos.position.z);

		treeAmount = Random.Range(100, 250);

		for (int i = 0; i < treeAmount; i++) {
			raycastOrigin = new Vector3(a_worldPos.position.x + Random.Range(0, 100), a_worldPos.position.y + 25, a_worldPos.position.z + Random.Range(0, 100));

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 25, terrainLayer) && hit.point.y > 6 && hit.point.y < 11) {
				currentTree = Instantiate(treePrefab);
				currentTree.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentTree.transform.parent = a_worldPos;
				currentTree.transform.position = hit.point;

				treeOffsetY = currentTree.transform.position.y + (currentTree.transform.localScale.y / 2);

				currentTree.transform.localPosition = new Vector3(raycastOrigin.x - a_worldPos.position.x, treeOffsetY, raycastOrigin.z - a_worldPos.position.z);

				currentTree.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
			}
		}
	}
}
