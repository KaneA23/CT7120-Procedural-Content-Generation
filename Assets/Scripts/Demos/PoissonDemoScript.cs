using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of objects in the Poisson Disc Sample demo scene.
/// </summary>
public class PoissonDemoScript : MonoBehaviour {
	[SerializeField] private Transform plane;
	[SerializeField] private GameObject[] treePrefab;

	private Transform currentObject;
	private RaycastHit hit;

	private Vector3 raycastOrigin;

	private float radius;
	private readonly int attemptAmount = 10;

	private List<Vector2> points;

	// Start is called before the first frame update
	void Start() {
		StartCoroutine(SpawnObjects());
	}

	/// <summary>
	/// Using Poisson generated points, spawns in creates with minimum distance from each other onto the plane
	/// </summary>
	/// <returns>Waits 0.1 seconds before continuing the for loop</returns>
	IEnumerator SpawnObjects() {
		radius = 10;

		points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);

		// Checks if each poing can be placed on a terrain then spawns in the object
		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(point.x, 5, point.y);

			// Checks whether object can be placed and spawns either tree or rock
			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit)) {
				currentObject = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)]).transform;
				
				currentObject.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + (currentObject.localScale.y / 2), hit.point.z), Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);
			}

			yield return new WaitForSeconds(0.1f);
		}

		Debug.Log("Done");
	}
}
