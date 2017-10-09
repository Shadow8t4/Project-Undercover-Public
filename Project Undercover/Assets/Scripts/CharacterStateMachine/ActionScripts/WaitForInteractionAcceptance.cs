using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/WaitForInteractionAcceptance")]
public class WaitForInteractionAcceptance : Action
{
    public override void StartAct(StateController controller)
    {
        controller.InitiateInteractionWithSelectedObject();
        ProgressPanelController.ActivePanel.Reveal(controller.SelectedInteraction.interactionDescription);
    }
}

