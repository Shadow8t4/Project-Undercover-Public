using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/InteractionAccepted")]
public class InteractionAcceptedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.IsInteractionAccepted();
    }
}