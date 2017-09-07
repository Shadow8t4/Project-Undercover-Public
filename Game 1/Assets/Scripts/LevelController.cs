using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public Text winText;
    protected static LevelController levelController;
    private bool wonLevel = false;

    void Start () {
		if (levelController)
        {
            Destroy(this);
        }
        levelController = this;
    }

    public static LevelController GetController()
    {
        if (levelController)
            return levelController;
        Debug.LogError("A LevelController object is not present in this scene.");
        return null;
    }

    public void WinLevel()
    {
        wonLevel = true;
        winText.text = "YOU WIN!";
        StartCoroutine(DelayLoadLevel("Level1", 3.0f));
    }

    public void ResetLevel()
    {
        if (wonLevel)
            return;
        winText.text = "TRY AGAIN!";
        StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().name, 2.0f));
    }

    public IEnumerator DelayLoadLevel(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
		if (SceneManager.GetActiveScene ().name == levelName) {
			// soft restart - just reset location
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			GameObject start = GameObject.FindGameObjectWithTag ("Start");
			player.transform.position = start.transform.position;
			winText.text = "";
		} else {
			SceneManager.LoadScene (levelName);
		}
        yield return null;
    }
}
