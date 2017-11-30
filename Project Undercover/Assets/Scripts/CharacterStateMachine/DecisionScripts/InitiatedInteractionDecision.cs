using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/InitiatedInteraction")]
public class InitiatedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.SelectedInteraction != null &&
            InteractionPanelController.InteractionPrompted() &&
            controller.SelectedObject != null &&
            controller.SelectedInteraction != null;
    }
}