using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class ControllerBiped : MonoBehaviour {

	private Dictionary<string, string> controls = new Dictionary<string, string>{
		{ "moveUp", "W" },
		{ "moveDown", "S" },
		{ "moveLeft", "A" },
		{ "moveRight", "D" },
		{ "jump", "Space" },
		{ "run", "LeftShift" }
	};

	private Dictionary<string, bool> abilities = new Dictionary<string, bool>{
		{ "jump", true },
		{ "groundJump", true },
		{ "wallJump", true },
		{ "airJump", true },
		{ "comboJump", true },
		{ "run", true }
	};

	private int inputZ = 0;
	private int inputX = 0;
	private bool inputComboZ = false;
	private bool inputComboX = false;
	private int inputComboSingletonZ = 0;
	private int inputComboSingletonX = 0;

	private bool running = false;
	private bool jumpGround = abilities["groundJump"];
	private bool jumpWall = abilities["wallJump"];
	private bool jumpAir = abilities["airJump"];
	private bool onGround = true;
	private bool onWall = false;

	public float speedMaxZ = 10f;
	public float speedMaxX = 10f;
	private float speedTargetZ = 0f;
	private float speedTargetX = 0f;
	private float speedCurrentZ = 0f;
	private float speedCurrentX = 0f;
	public float speedRunning = 2f; // Running multiplier
	public float speedWallY = 10f; // Wall walking anti-gravity force
	public float speedJump = 20f; // Vertical jumping impulse
	public float speedComboJump = 30f; // Horizontal combo jumping impulse

	private Rigidbody rgb;
	private ConstantForce cf;

	private void Awake () {
		rgb = GetComponent<Rigidbody>();
		cf = GetComponent<ConstantForce>();
	}

	private void Update () {
		CaptureInput();
	}

	private void FixedUpdate () {
		speedTargetX = inputX * speedMaxX * speedRunning * (!onGround && !onWall ? 0.2f : 1f) * -1f;
		speedTargetZ = inputZ * speedMaxZ * speedRunning * (!onGround && !onWall ? 0.2f : 1f);

		speedCurrentX = Mathf.Lerp(speedCurrentX, speedTargetX, Time.fixedTime);
		float y = onWall ? speedWallY : 0f;
		speedCurrentZ = Mathf.Lerp(speedCurrentZ, speedTargetZ, Time.fixedTime);

		cf.relativeForce = new Vector3(speedCurrentX, y, speedCurrentZ);
	}

	private void CaptureInput () {
		// X
		if (Input.GetKeyDown(controls["moveLeft"])) {
			inputX = 1;
			StartCoroutine(SetMoveCombo(true));
		}
		else if (Input.GetKeyDown(controls["moveRight"])) {
			inputX = -1;
			StartCoroutine(SetMoveCombo(true));
		}
		else if ((Input.GetKeyUp(controls["moveLeft"]) || Input.GetKeyUp(controls["moveRight"])) && inputX != 0) {
			inputX = 0;
		}
		// Z
		if (Input.GetKeyDown(controls["moveUp"])) {
			inputZ = 1;
			StartCoroutine(SetMoveCombo(false));
		}
		else if (Input.GetKeyDown(controls["moveDown"])) {
			inputZ = -1;
			StartCoroutine(SetMoveCombo(false));
		}
		else if ((Input.GetKeyUp(controls["moveUp"]) || Input.GetKeyUp(controls["moveDown"])) && inputZ != 0) {
			inputZ = 0;
		}
		// Jump
		if (Input.GetKeyDown(controls["jump"])) {
			Jump();
		}
		// Run
		if (Input.GetKeyDown(controls["run"]) && abilities["run"]) {
			running = true;
		}
		else if (Input.GetKeyUp(controls["run"])) {
			running = false;
		}
	}

	private IEnumerator SetMoveCombo (bool isX) {
		if (isX) {
			inputComboX = true;
			inputComboSingletonX++;
			yield return new WaitForSeconds(0.1f);
			inputComboX = false;
			if (inputComboSingletonX == 1) {
				inputComboX = false;
			}
			inputComboSingletonX--;
		}
		else {
			inputComboZ = true;
			inputComboSingletonZ++;
			yield return new WaitForSeconds(0.1f);
			if (inputComboSingletonZ == 1) {
				inputComboZ = false;
			}
			inputComboSingletonZ--;
		}
	}

	private void Jump () {
		if (jumps > 0 && abilities["jump"]) {
			// Ground
			if (onGround && abilities["groundJump"]) {
				// Combo jump
				if (abilities["comboJump"] && inputComboX) {
					rgb.AddForce(transform.right * inputX * speedComboJump * -1f, ForceMode.Impulse);
				}
				if (abilities["comboJump"] && inputComboZ) {
					rgb.AddForce(transform.forward * inputZ * speedComboJump, ForceMode.Impulse);
				}
				// Normal jump
				rgb.AddForce(transform.up * speedJump, ForceMode.Impulse);
				jumpGround = false;
			}
			// Wall
			else if (onWall && abilities["wallJump"]) {
				// Combo wall jump
				if (abilities["comboJump"] && inputComboX) {
					rgb.AddForce(transform.right * inputX * speedComboJump * -1f, ForceMode.Impulse);
				}
				if (abilities["comboJump"] && inputComboZ) {
					rgb.AddForce(transform.forward * inputZ * speedComboJump, ForceMode.Impulse);
				}
				// Normal wall jump
				rgb.AddForce(transform.up * speedJump, ForceMode.Impulse);
				jumpWall = false;
			}
			// Air
			else if (!onGround && !onWall && abilities["airJump"]) {
				// Combo air jump
				if (abilities["comboJump"] && inputComboX) {
					rgb.AddForce(transform.right * inputX * speedComboJump * -1f, ForceMode.Impulse);
				}
				if (abilities["comboJump"] && inputComboZ) {
					rgb.AddForce(transform.forward * inputZ * speedComboJump, ForceMode.Impulse);
				}
				// Normal air jump
				rgb.AddForce(transform.up * speedJump, ForceMode.Impulse);
				jumpAir = false;
			}
		}
	}

	public void EnterGround () {
		onGround = true;
		jumpGround = abilities["groundJump"];
		jumpWall = abilities["wallJump"];
		jumpAir = abilities["airJump"];
	}

	public void ExitGround () {
		onGround = false;
	}

	public void HitWall () {
		onWall = true;
		jumpWall = abilities["wallJump"];
		jumpAir = abilities["airJump"];
	}

	public void ExitWall () {
		onWall = false;
	}
}