using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanelController : MonoBehaviour {

    public Text progressText;
    public GameObject progressBar, mainPanel;
    private float maxWidth;

    [Range(0, 1)]
    private float progress = 0.0f;
    private HSBColor startColor, endColor;

    private static ProgressPanelController activePanel;

    void Start() {
        maxWidth = progressBar.GetComponent<RectTransform>().sizeDelta.x;
        startColor = HSBColor.FromColor(Color.red);
        endColor = HSBColor.FromColor(Color.blue);
        ActivePanel = this;
        Hide();
    }

    public void Reveal(string progressText)
    {
        this.progressText.text = progressText;
        mainPanel.SetActive(true);
        Progress = 0.0f;
    }

    public void Hide()
    {
        mainPanel.SetActive(false);
        Progress = 0.0f;
    }

    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = Mathf.Clamp01(value);
            var rectTrans = progressBar.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(progress * maxWidth, rectTrans.sizeDelta.y);

            var bar = progressBar.GetComponent<Image>();
            bar.color = HSBColor.Lerp(startColor, endColor, progress).ToColor();
        }

    }

    public static ProgressPanelController ActivePanel
    {
        get
        {
            if (activePanel)
                return activePanel;
            Debug.LogError("No panel in scene");
            return null;
        }
        set
        {
            if (!activePanel)
                activePanel = value;
            else
            {
                Debug.LogError("More than one progress panel in the scene");
            }
        }
    }
}
