using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/PromptInteractions")]
public class PromptInteractionsAction : Action {

    public override void Act(StateController controller)
    {
        string objectInteractionText = "Press 'E' to interact with ";
        if (ReceivedInteraction(controller))
        {
            InteractionPanelController.ActivePanel.Reveal(controller.Interactor.interactionText);
        }
        else if (SelectedObjectAvailable(controller))
        {
            InteractionPanelController.ActivePanel.Reveal(objectInteractionText + controller.SelectedObject.GetInteractionTitle());
        }
        else
        {
            InteractionPanelController.ActivePanel.Hide();
        }
    }

    public override void EndAct(StateController controller)
    {
        InteractionPanelController.ActivePanel.Hide();
    }

    // Other controller is attempting to interact with this controller
    bool ReceivedInteraction(StateController controller)
    {
        return controller.Interactor;
    }

    bool SelectedObjectAvailable(StateController controller)
    {
        if (controller.SelectedObject == null)
            return false;
        return (controller.SelectedObject.transform.position - controller.transform.position).magnitude < controller.INTERACT_RANGE;
    }
}
