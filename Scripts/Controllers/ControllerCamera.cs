using UnityEngine;

public class ControllerCamera : MonoBehaviour {

	public float sensitivityX = 10f;
	public float sensitivityY = 10f;

	private float minimumY = -60f;
	private float maximumY = 60f;

	private float rotationY = 0f;

	private void Update () {
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		transform.rotation = Qauternion.Euler(new Vector3(-rotationY, rotationX, 0));
	}

}