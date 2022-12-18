using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the spawning of objects in the Poisson Disc Sample demo scene.
/// </summary>
public class PoissonDemoScript : MonoBehaviour {
	[SerializeField] private Transform plane;
	[SerializeField] private GameObject[] treePrefab;

	[SerializeField] private Slider attemptSlider;

	private List<GameObject> spawnedObjects;

	private Transform currentObject;
	private RaycastHit hit;

	private Vector3 raycastOrigin;

	private float radius;
	private int attemptAmount = 30;

	private List<Vector2> points;

	[SerializeField] private bool usePoisson;

	// Start is called before the first frame update
	void Start() {
		points = new List<Vector2>();
		spawnedObjects = new List<GameObject>();

		attemptAmount = 30;
		radius = 10f;

		usePoisson = true;

		StartCoroutine(SpawnObjects());
	}

	public void OnGenerateButtonPressed() {
		StopAllCoroutines();
		RemoveOldObjects();
		StartCoroutine(SpawnObjects());
	}

	void RemoveOldObjects() {
		foreach (GameObject obj in spawnedObjects) {
			Destroy(obj);
		}

		spawnedObjects.Clear();
		points.Clear();
	}

	/// <summary>
	/// Spawns objects onto plane either randomly or using Poisson Disc Sampling
	/// </summary>
	/// <returns>Waits 0.1 seconds before continuing the for loop</returns>
	IEnumerator SpawnObjects() {
		//radius = 7.5f;//Random.Range(7.5f, 10f);

		if (usePoisson) {
			points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);
		} else {
			for (int i = 0; i < 100/*Random.Range(75, 100)*/; i++) {
				points.Add(new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f)));
			}
		}

		// Checks if each poing can be placed on a terrain then spawns in the object
		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(point.x, 5, point.y);

			// Checks whether object can be placed and spawns either tree or rock
			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit)) {
				currentObject = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)]).transform;

				currentObject.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + (currentObject.localScale.y / 2), hit.point.z), Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);

				spawnedObjects.Add(currentObject.gameObject);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	public float Radius {
		get {
			return radius;
		}

		set {
			radius = value;
		}
	}

	public bool UsePoisson {
		get {
			return usePoisson;
		}

		set {
			usePoisson = value;
		}
	}

	public void ChangeAttemptAmount() {
		attemptAmount = (int)attemptSlider.value;
	}
}
