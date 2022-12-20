using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of trees and other gameobjects within a meshed area.
/// </summary>
public class ObjectSpawner : MonoBehaviour {
	[SerializeField] private GameObject[] objPrefabs;

	private Transform currentObj;
	private RaycastHit hit;
	private Vector3 raycastOrigin;

	private float yOffset;

	private float radius;
	private readonly int attemptAmount = 30;

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
	/// <param name="a_worldPos">Mesh that objects are to be spawned on</param>
	/// <param name="a_objectToSpawn">What environment item is trying to be spawned</param>
	public void SpawnObjects(Transform a_worldPos, EnvProp a_objectToSpawn, float a_minObjectPlacement, float a_maxObjectPlacement) {
		radius = Random.Range(10, 20);

		points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);

		// Checks whether the generated points can be placed on terrain
		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(a_worldPos.position.x + point.x, 100, a_worldPos.position.z + point.y);

			// If raycasted point is within a set region on terrain, spawn in object
			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit) && hit.point.y > a_minObjectPlacement && hit.point.y < a_maxObjectPlacement) {
				currentObj = Instantiate(objPrefabs[(int)a_objectToSpawn]).transform;
				currentObj.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

				currentObj.parent = a_worldPos;
				currentObj.position = hit.point;

				// Trees have to stand up, rocks can be at any angle
				if (currentObj.localScale.y > 1) {
					yOffset = currentObj.position.y + 1;
					currentObj.localRotation = Quaternion.Euler(0, currentObj.localRotation.y, 0);
				} else {
					yOffset = currentObj.position.y + (currentObj.localScale.y / 2);
				}

				currentObj.localPosition = new Vector3(raycastOrigin.x - a_worldPos.position.x, yOffset, raycastOrigin.z - a_worldPos.position.z);
			}
		}
	}
}
