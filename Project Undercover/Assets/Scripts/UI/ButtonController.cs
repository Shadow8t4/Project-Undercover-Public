using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
	public void ExitGame() {
		Application.Quit();
	}
	
	public void NextScene(string name) {
		Application.LoadLevel(name);
	}
}
