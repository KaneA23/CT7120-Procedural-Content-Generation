using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour {
	[SerializeField] private GameObject treePrefab;

	private GameObject currentTree;
	RaycastHit hit;
	Vector3 raycastOrigin;

	[SerializeField] private LayerMask terrainLayer;
	int treeAmount;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void SpawnTrees(Transform a_worldPos) {
		//Vector3 raycastOrigin = new Vector3(a_worldPos.position.x, a_worldPos.position.y + 10, a_worldPos.position.z);

		treeAmount = Random.Range(100, 250);

		for (int i = 0; i < treeAmount; i++) {
			raycastOrigin = new Vector3(Random.Range(0, 100), a_worldPos.position.y + 25, Random.Range(0, 100));


			Debug.Log("Spawning trees at: " + raycastOrigin.ToString());

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 25, terrainLayer) && hit.point.y > 6 && hit.point.y < 11) {
				currentTree = Instantiate(treePrefab);
				currentTree.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentTree.transform.parent = a_worldPos;
				currentTree.transform.position = hit.point;

				currentTree.transform.localPosition = new Vector3(raycastOrigin.x, currentTree.transform.position.y /*+ (currentTree.transform.localScale.y / 2)*/, raycastOrigin.z);

				currentTree.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
			}
		}
	}
}
