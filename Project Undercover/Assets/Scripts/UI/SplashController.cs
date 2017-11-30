using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour {
	private Image splashText;
	private float timer;

	void Start () {
		splashText = GetComponent<Image> ();
		Color tempcolor = Color.white;
		tempcolor.a = 0;
		splashText.color = tempcolor;
		timer = 4;
	}

	void Update () {
		Color setalpha = Color.white;
		timer -= Time.deltaTime;

		if(timer >= 2.5 && timer < 3.5) {
			setalpha.a = 1 - (timer - 2.5f);
			splashText.color = setalpha;
		}
		if(timer >= 0.5 && timer < 1.5) {
			setalpha.a = (timer - 0.5f);
			splashText.color = setalpha;
		}
		if(timer <= 0) {
			SceneManager.LoadScene ("MainMenu");
		}
	}
}