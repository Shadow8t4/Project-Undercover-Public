using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/ReceivingInteraction")]
public class ReceivingInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.Interactor != null;
    }
}