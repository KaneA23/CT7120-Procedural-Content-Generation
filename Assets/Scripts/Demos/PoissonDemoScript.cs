using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Update is called once per frame
	void Update() {

	}

	IEnumerator SpawnObjects() {
		radius = Random.Range(7.5f, 10);

		points = PoissonDiscSampler.GeneratePoints(radius, 100, attemptAmount);

		foreach (Vector2 point in points) {
			Debug.Log("Raycast: " + raycastOrigin.ToString());
			raycastOrigin = new Vector3(/*plane.position.x + */point.x, 5, /*plane.position.z + */point.y);

			if (Physics.Raycast(raycastOrigin, Vector3.down, out hit)) {
				currentObject = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)]).transform;
				
				currentObject.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + (currentObject.localScale.y / 2), hit.point.z), Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);
			}

			yield return new WaitForSeconds(0.1f);
		}

		Debug.Log("Done");
	}
}
