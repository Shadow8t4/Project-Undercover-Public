using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/InitiatedInteraction")]
public class InitiatedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Input.GetKeyDown(KeyCode.E) &&
            InteractionPanelController.InteractionPrompted() &&
            controller.SelectedObject != null;/* &&
            !controller.SelectedObject.IsInteracting &&
            controller.SelectedObject.Interactor == null;*/
    }
}