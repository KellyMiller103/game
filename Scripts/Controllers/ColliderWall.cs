using UnityEngine;

public class ColliderWall : MonoBehaviour {

	private Controller controller;

	private void Awake () {
		controller = transform.parent.GetComponent<Controller>();
	}

	private void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Terrain") {
			controller.EnterWall();
		}
	}

	private void OnCollisionExit (Collision collision) {
		if (collision.gameObject.tag == "Terrain") {
			controller.ExitWall();
		}
	}
}