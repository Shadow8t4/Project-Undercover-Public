using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/PromptInteractions")]
public class PromptInteractionsAction : Action {

    public override void Act(StateController controller)
    {
        if (ReceivedInteraction(controller))
        {
            InteractionPanelController.ActivePanel.AcceptInteractionReveal(controller);
        }
        else if (SelectedObjectAvailable(controller))
        {
            InteractionPanelController.ActivePanel.SelectInteractionReveal(controller);
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
        if (controller.SelectedObject.Interactor != null)
            return false;
        return (controller.SelectedObject.transform.position - controller.transform.position).magnitude < StateController.INTERACT_RANGE;
    }
}
