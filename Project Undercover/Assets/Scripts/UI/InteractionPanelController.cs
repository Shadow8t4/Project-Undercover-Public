using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanelController : MonoBehaviour {

    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private Dropdown interactionsDropdown;
    [SerializeField]
    private GameObject interactionItemPrefab;
    [SerializeField]
    private GameObject requestPanel;
    [SerializeField]
    private Text requestedInteractionText;
    private static InteractionPanelController activePanel;
    private StateController _controller;

    public class InteractionData : Dropdown.OptionData
    {
        public Interaction interaction;
        public bool isSpyInteraction;
        public InteractionData(Interaction interaction, bool isSpyInteraction)
        {
            this.interaction = interaction;
            this.isSpyInteraction = isSpyInteraction;
            text = interaction.interactionDescription;
            if (isSpyInteraction)
                text = text + " (Spy)";
        }
    }

    void Start()
    {
        ActivePanel = this;
        Hide();
    }

    public void SelectInteractionReveal(StateController controller)
    {
        _controller = controller;
        if (mainPanel.activeInHierarchy)
            return;
        interactionsDropdown.ClearOptions();

        var optionsList = new List<Dropdown.OptionData>();
        foreach (Interaction interaction in controller.SelectedObject.interactions)
        {
            var data = new InteractionData(interaction, false);
            optionsList.Add(data);
        }
        if (controller.SelectedObject.CompareTag("Spy"))
        {
            foreach (Interaction interaction in controller.SelectedObject.spyInteractions)
            {
                var data = new InteractionData(interaction, true);
                optionsList.Add(data);
            }
        }

        interactionsDropdown.AddOptions(optionsList);
        if (interactionsDropdown.value > interactionsDropdown.options.Count)
            interactionsDropdown.value = 0;
        _controller.SelectedInteraction = ((InteractionData)(interactionsDropdown.options[interactionsDropdown.value])).interaction;
        ActivePanel.mainPanel.SetActive(true);
    }

    public void SetSelectedInteraction()
    {
        _controller.SelectedInteraction = ((InteractionData)(interactionsDropdown.options[interactionsDropdown.value])).interaction;
    }

    public void AcceptInteractionReveal(StateController controller)
    {
        _controller = controller;
        requestPanel.SetActive(true);
        string description = controller.Interactor.SelectedInteraction.receiverDescription;
        requestedInteractionText.GetComponent<Text>().text = "Press 'E' to " + description;
    }

    public void Hide()
    {
        mainPanel.SetActive(false);
        requestPanel.SetActive(false);
        interactionsDropdown.Hide();
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