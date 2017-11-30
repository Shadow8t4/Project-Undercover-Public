using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour {

    public static bool MenuBlocked { get; set; }

	public GameObject inGameMenu;

    private void Start() {
        MenuBlocked = false;
    }

    void Update() {
        if (MenuBlocked) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            EscapePressed();
        }
	}

	void EscapePressed() {
        if (inGameMenu.GetActive()) {
            Resume();
        } else {
		    inGameMenu.SetActive(true);
        }
	}

	public void Resume() {
		inGameMenu.SetActive(false);
	}
}
