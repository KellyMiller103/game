using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Need to create new gameobjects: Button, Text, etc
	// If consoleY is lower than screen height then start increasing parent height

	public Transform consoleParent;

	public GameObject prfText;
	public GameObject prfButton;
	public GameObject prfInput;

	private float consoleY;

	private void Start () {
		StartCoroutine(Intro());
	}

	private IEnumerator Intro () {
		StartCoroutine(Statics.ConsoleText(TextConsole, "Booting ", 0.1f, true));
		yield return new WaitForSeconds(0.1f);
		TextConsole.text += "."
		yield return new WaitForSeconds(Random.value * 0.4f);
		TextConsole.text += "."
		yield return new WaitForSeconds(Random.value * 0.4f);
		TextConsole.text += "."
		yield return new WaitForSeconds(Random.value * 0.4f);
		StartCoroutine(Statics.ConsoleText(TextConsole, "Welcome to MechDrone game!\n", 0.1f, true));
	}

	public IEnumerator ConsoleText (Text textUI, string textRaw, float rate = 0.1f, bool override = false) {
		string[] textArray = textRaw.Split["\n"];
		for (int i = 0; i < textArray.Length; i++) {
			if (override) {
				textUI.text = "";
			}
			else {
				textUI.text += "\n";
			}
			float lineRate = rate / textArray[i].Length;
			for (int j = 0; j < textArray[i].Length; j++) {
				textUI.text += System.Convert.ToString(textArray[i][j]);
				yield return new WaitForSeconds(lineRate);
			}
		}
	}

}