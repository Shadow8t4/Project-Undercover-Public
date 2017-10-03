using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanelController : MonoBehaviour {

    public Text interactionText;
    public GameObject  mainPanel;
    private static InteractionPanelController activePanel;

    void Start()
    {
        ActivePanel = this;
        Hide();
    }

    public void Reveal(string interactionText)
    {
        this.interactionText.text = interactionText;
        mainPanel.SetActive(true);
    }

    public void Hide()
    {
        mainPanel.SetActive(false);
    }

    public static InteractionPanelController ActivePanel
    {
        get
        {
            if (activePanel)
                return activePanel;
            Debug.LogError("No interaction panels in scene");
            return null;
        }
        set
        {
            if (!activePanel)
                activePanel = value;
            else
            {
                Debug.LogError("More than one interaction panel currently exists in the scene");
            }
        }
    }
}