using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player movement behaviours in a first person character.
/// </summary>
public class PlayerMovement : MonoBehaviour {
	private float speed = 10f;

	private float hMovement;
	private float vMovement;

	private Rigidbody rb;
	private Vector3 moveDir;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	private void Start() {
		// Generates player to be on top of the ground instead of spawning in the sky
		if (Physics.Raycast(new Vector3(50, 25, 50), Vector3.down, out RaycastHit hit)) {
			transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
		}
	}

	// Update is called once per frame
	private void Update() {
		PlayerInput();
	}

	private void FixedUpdate() {
		MovePlayer();
	}

	/// <summary>
	/// Collects data on what buttons player are pressing
	/// </summary>
	private void PlayerInput() {
		hMovement = Input.GetAxisRaw("Horizontal");
		vMovement = Input.GetAxisRaw("Vertical");
	}

	/// <summary>
	/// Moves player character in a direction dependent on player input
	/// </summary>
	private void MovePlayer() {
		moveDir = transform.forward * vMovement + transform.right * hMovement;

		rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);
	}
}
