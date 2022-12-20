using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the spawning of objects in the Poisson Disc Sample demo scene.
/// </summary>
public class PoissonDemoScript : MonoBehaviour {
	[Header("Prefabs")]
	[SerializeField] private GameObject[] objPrefabs;

	[Header("Algorithm Customisation")]
	[SerializeField] private Slider attemptSlider;
	[SerializeField] private Slider radiusSlider;

	private float radius;
	private int attemptAmount;
	private bool usePoisson;    // Checks whether the algorithm is being used or random?

	private List<GameObject> spawnedObjects;

	private Transform currentObject;
	private RaycastHit hit;

	private Vector3 raycastOrigin;

	private List<Vector2> points;

	// Start is called before the first frame update
	private void Start() {
		points = new List<Vector2>();
		spawnedObjects = new List<GameObject>();

		attemptAmount = 30;
		radius = 10f;

		attemptSlider.value = attemptAmount;
		radiusSlider.value = radius;

		usePoisson = true;
	}

	/// <summary>
	/// Restarts generation with given values
	/// </summary>
	public void OnGenerateButtonPressed() {
		StopAllCoroutines();
		RemoveOldObjects();
		StartCoroutine(SpawnObjects());
	}

	/// <summary>
	/// Remoces all objects that were previously spawned in
	/// </summary>
	private void RemoveOldObjects() {
		foreach (GameObject obj in spawnedObjects) {
			Destroy(obj);
		}

		spawnedObjects.Clear();
	}

	/// <summary>
	/// Spawns objects onto plane either randomly or using Poisson Disc Sampling
	/// </summary>
	/// <returns>Waits 0.1 seconds before continuing the for loop</returns>
	private IEnumerator SpawnObjects() {
		points.Clear();

		if (usePoisson) {
			points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);
		} else {
			for (int i = 0; i < 100; i++) {
				points.Add(new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f)));
			}
		}

		// Checks if each poing can be placed on a terrain then spawns in the object
		foreach (Vector2 point in points) {
			raycastOrigin = new Vector3(point.x, 5, point.y);

			// Checks whether object can be placed and spawns either tree or rock
			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit)) {
				currentObject = Instantiate(objPrefabs[Random.Range(0, objPrefabs.Length)]).transform;

				currentObject.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + (currentObject.localScale.y / 2), hit.point.z), Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);

				spawnedObjects.Add(currentObject.gameObject);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	#region Sliders

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

	/// <summary>
	/// Changes how many times the poisson sampler runs before discarding point
	/// </summary>
	public void ChangeAttemptAmount() {
		attemptAmount = (int)attemptSlider.value;
	}

	#endregion Sliders
}
