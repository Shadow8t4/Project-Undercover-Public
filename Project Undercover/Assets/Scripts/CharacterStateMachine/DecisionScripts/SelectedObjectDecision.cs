using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/SelectedObject")]
public class SelectedObjectDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.SelectedObject != null && controller.SelectedInteraction != null;
    }
}

