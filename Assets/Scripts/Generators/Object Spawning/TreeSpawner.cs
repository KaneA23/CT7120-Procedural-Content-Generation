using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of trees and other gameobjects within a meshed area.
/// </summary>
public class TreeSpawner : MonoBehaviour {
	[SerializeField] private GameObject[] treePrefabs;

	private Transform currentObject;
	private RaycastHit hit;
	private Vector3 raycastOrigin;

	private float yOffset;

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

	/// <summary>
	/// From list of points generated, checks whether object can spawn on created terrain and then spawns it
	/// </summary>
	/// <param name="a_worldPos">Mesh that trees are to be spawned on</param>
	/// <param name="a_objectToSpawn">What environment item is trying to be spawned</param>
	public void SpawnObjects(Transform a_worldPos, EnvProp a_objectToSpawn, float a_minObjectPlacement, float a_maxObjectPlacement) {
		radius = Random.Range(10, 20);

		points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);

		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(a_worldPos.position.x + point.x, 100, a_worldPos.position.z + point.y);

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit) && hit.point.y > a_minObjectPlacement && hit.point.y < a_maxObjectPlacement) {
				currentObject = Instantiate(treePrefabs[(int)a_objectToSpawn]).transform;
				currentObject.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentObject.parent = a_worldPos;
				currentObject.position = hit.point;

				//if (currentObject.localScale.y > 1) {
					//yOffset = currentObject.position.y + 1;
					//currentObject.localRotation = Quaternion.Euler(0, currentObject.localRotation.y, 0);
				//} else {
					yOffset = currentObject.position.y + (currentObject.localScale.y / 2);
				//}

				currentObject.localPosition = new Vector3(raycastOrigin.x - a_worldPos.position.x, yOffset, raycastOrigin.z - a_worldPos.position.z);
			}
		}
	}
}
