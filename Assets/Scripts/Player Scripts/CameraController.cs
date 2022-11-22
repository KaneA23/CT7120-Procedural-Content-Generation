using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[Header("Camera Settings")]
	[SerializeField] private float mouseSensitivity = 10f;

	private float mouseX;
	private float mouseY;
	float xRotation = 0f;

	private Transform playerBody;

	private void Awake() {
		playerBody = transform.parent;
	}

	// Start is called before the first frame update
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update() {
		PlayerInput();
		RotateCamera();
	}

	void PlayerInput() {
		mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
		mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
	}

	void RotateCamera() {
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		playerBody.Rotate(Vector3.up * mouseX);
	}
}
