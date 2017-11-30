using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanelController : MonoBehaviour {

    // One choice vars
    [SerializeField]
    GameObject oneChoicePanel;
    [SerializeField]
    InteractionButtonProperties interactionButton;

    // two choices vars
    [SerializeField]
    GameObject choicePanel;
    [SerializeField]
    InteractionButtonProperties spyButton;
    [SerializeField]
    InteractionButtonProperties npcButton;

    private static InteractionPanelController activePanel;
    private StateController _controller;

    void Start()
    {
        ActivePanel = this;
        Hide();
    }

    public void SelectInteractionReveal(StateController controller)
    {
        _controller = controller;
        if (oneChoicePanel.activeInHierarchy || choicePanel.activeInHierarchy)
            return;

        int randomInt = (int)(Random.value * controller.SelectedObject.interactions.Length);
        Interaction randomNpcInteraction = controller.SelectedObject.interactions[randomInt];

        if (controller.SelectedObject.spyInteractions.Length == 0)
        {
            oneChoicePanel.SetActive(true);
            interactionButton.SetInteraction(randomNpcInteraction);
        }
        else
        {
            choicePanel.SetActive(true);
            Interaction spyInteraction = controller.SelectedObject.spyInteractions[0];
            spyButton.SetInteraction(spyInteraction);
            npcButton.SetInteraction(randomNpcInteraction);
        }
    }

    public void AcceptInteractionReveal(StateController controller)
    {
        _controller = controller;
        oneChoicePanel.SetActive(true);
        choicePanel.SetActive(false);
        string description = controller.Interactor.SelectedInteraction.receiverDescription;
        interactionButton.text.text = description;
        interactionButton.SetInteraction(controller.Interactor.SelectedInteraction);
    }

    public void Hide()
    {
        choicePanel.SetActive(false);
        oneChoicePanel.SetActive(false);
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
        return ActivePanel.choicePanel.activeInHierarchy || ActivePanel.oneChoicePanel.activeInHierarchy;
    }

    public void SetInteraction(Interaction interaction)
    {
        _controller.SelectedInteraction = interaction;
    }

    /*
    public bool WasInteractionInitiated()
    {
        if (_initiatedInteraction)
        {
            _initiatedInteraction = false;
            return _initiatedInteraction;
        }
        return false;
    }

    public void InitiatedInteraction()
    {
        _initiatedInteraction = true;
    }*/
}