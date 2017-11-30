using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour {

	public GameObject inGameMenu;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape ))
			EscapePressed();
	}

	void EscapePressed() {
		inGameMenu.SetActive (true);
	}

	public void Resume() {
		inGameMenu.SetActive (false);
	}
}
