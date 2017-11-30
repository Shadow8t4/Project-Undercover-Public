using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButtonProperties : MonoBehaviour {

    public Text text;

    public Interaction interaction;

	void Awake () {

        if (transform.childCount > 0)
        {
            text = transform.GetChild(0).GetComponent<Text>();
        }
        if (text == null)
            Debug.LogError("No text child object iside of this interaction button");
	}

    public void SetInteraction(Interaction inter)
    {
        interaction = inter;
        text.text = interaction.interactionDescription;
    }
	
    public void InteractionSelected()
    {
        Debug.Log("Selected interaction " + interaction.name + " from button " + name);
        InteractionPanelController.ActivePanel.SetInteraction(interaction);
    }

}
