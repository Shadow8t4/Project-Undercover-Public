using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/WaitForInteractionAcceptance")]
public class WaitForInteractionAcceptance : Action
{
    public override void StartAct(StateController controller)
    {
        // Todo: Remove this line once the "interaction selector" UI is finished
        controller.SelectedInteraction = GetFirstInteraction(controller);
        controller.InitiateInteractionWithSelectedObject();
        ProgressPanelController.ActivePanel.Reveal(controller.SelectedInteraction.interactionDescription);
    }

    public override void EndAct(StateController controller)
    {
        //Debug.Log("Finished waiting for acceptance");
    }

    private Interaction GetFirstInteraction(StateController controller)
    {
        if (controller.SelectedObject.interactions.Length > 0)
            return controller.SelectedObject.interactions[0];
        return controller.SelectedObject.spyInteractions[0];
    }
}

