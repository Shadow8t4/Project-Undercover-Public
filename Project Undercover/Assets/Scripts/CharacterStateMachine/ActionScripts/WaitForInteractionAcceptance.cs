using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/WaitForInteractionAcceptance")]
public class WaitForInteractionAcceptance : Action
{
    public override void StartAct(StateController controller)
    {
        // Todo: Remove this line once the "interaction selector" UI is finished
        controller.SelectedInteraction = controller.SelectedObject.interactions[0];
        controller.InitiateInteractionWithSelectedObject();
        ProgressPanelController.ActivePanel.Reveal(controller.SelectedInteraction.interactionDescription);
    }

    public override void EndAct(StateController controller)
    {
        //Debug.Log("Finished waiting for acceptance");
    }
}

