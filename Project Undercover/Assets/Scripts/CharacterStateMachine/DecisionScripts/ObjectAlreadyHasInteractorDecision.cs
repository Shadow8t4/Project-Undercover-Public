using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/ObjectAlreadyHasInteractor")]
public class ObjectAlreadyHasInteractorDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.SelectedObject.Interactor != null)
            return controller.SelectedObject.Interactor != controller;
        return false;
    }
}
