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

    public static void Reveal(string interactionText)
    {
        ActivePanel.interactionText.text = interactionText;
        ActivePanel.mainPanel.SetActive(true);
    }

    public static void Hide()
    {
        ActivePanel.mainPanel.SetActive(false);
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

    public static bool InteractionPrompted()
    {
        return ActivePanel.mainPanel.activeInHierarchy;
    }
}