using UnityEngine;

public class ColliderGround : MonoBehaviour {

	private Controller controller;

	private void Awake () {
		controller = transform.parent.GetComponent<Controller>();
	}

	private void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Terrain") {
			controller.EnterGround();
		}
	}

	private void OnCollisionExit (Collision collision) {
		if (collision.gameObject.tag == "Terrain") {
			controller.ExitGround();
		}
	}
}