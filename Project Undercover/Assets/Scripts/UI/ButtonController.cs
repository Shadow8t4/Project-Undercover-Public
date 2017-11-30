using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

	public GameObject ui;

	public void ExitGame() {
		Application.Quit();
	}

	public void NextScene(string name) {
		Application.LoadLevel(name);
	}

	public void ShowInstructions() {
		ui.SetActive (true);
	}

	public void HideInstructions() {
		ui.SetActive (false);
	}
}
