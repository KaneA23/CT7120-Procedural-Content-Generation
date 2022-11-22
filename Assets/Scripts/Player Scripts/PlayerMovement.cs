using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float speed = 15f;

	private float hMovement;
	private float vMovement;

	Vector3 moveDir;

	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	private void Start() {

	}

	// Update is called once per frame
	private void Update() {
		PlayerInput();
	}

	private void FixedUpdate() {
		MovePlayer();
	}

	private void PlayerInput() {
		//if (Input.GetKey(KeyCode.A)) {
		//	hMovement = -1;
		//} else if (Input.GetKey(KeyCode.D)) {
		//	hMovement = 1;
		//} else {
		//	hMovement = 0;
		//}
		//
		//if (Input.GetKey(KeyCode.S)) {
		//	vMovement = -1;
		//} else if (Input.GetKey(KeyCode.W)) {
		//	vMovement = 1;
		//} else {
		//	vMovement = 0;
		//}

		hMovement = Input.GetAxisRaw("Horizontal");
		vMovement = Input.GetAxisRaw("Vertical");
	}

	private void MovePlayer() {
		moveDir = transform.forward * vMovement + transform.right * hMovement;

		rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);
	}
}
