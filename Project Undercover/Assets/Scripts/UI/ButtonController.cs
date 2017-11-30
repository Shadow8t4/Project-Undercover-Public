using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public AudioClip button_sound;
    public AudioSource button_sound_src;
    public GameObject ui;

    public void Start() {
        button_sound_src = GetComponent<AudioSource>();
    }

	public void ExitGame() {
        AudioManager.Main.PlayNewSound("button_sound");
		Application.Quit();
	}

	public void NextScene(string name)
    {
        AudioManager.Main.PlayNewSound("button_sound");
        Application.LoadLevel(name);
	}

	public void ShowInstructions() {

        AudioManager.Main.PlayNewSound("button_sound");
        ui.SetActive (true);
	}

	public void HideInstructions() {

        AudioManager.Main.PlayNewSound("button_sound");
        ui.SetActive (false);
	}
}
