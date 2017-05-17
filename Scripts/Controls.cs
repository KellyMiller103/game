using UnityEngine;
using System.Collections.Generic;

public static class Controls {
	public static Dictionary<string,string[]> keys = new Dictionary<string, string>{
	// Movement
		{ "moveUp", new string[] { "W" } },
		{ "moveDown", new string[] { "S" } },
		{ "moveLeft", new string[] { "A" } },
		{ "moveRight", new string[] { "D" } },
		{ "jump", new string[] { "Space" } },
		{ "run", new string[] { "LeftShift" } },
	// Weapons
		{ "fire", new string[] { "Mouse0" } }
	};

	public static bool Input (string type = "GetKeyDown", string key) {
		string[] keyLength = keys[key].Length;
		if (type == "GetKey") {
			if (keyLength == 0) {
				return false;
			}
			else if (keyLength == 1) {
				return Input.GetKey(keys[key][0]);
			}
			else {
				return Input.GetKey(keys[key][0]) || Input.GetKey(keys[key][1]);
			}
		}
		else if (type == "GetKeyDown") {
			if (keyLength == 0) {
				return false;
			}
			else if (keyLength == 1) {
				return Input.GetKeyDown(keys[key][0]);
			}
			else {
				return Input.GetKeyDown(keys[key][0]) || Input.GetKeyDown(keys[key][1]);
			}
		}
	}

	private static void KeyCheck (string[] keyCodes) {

	}

}