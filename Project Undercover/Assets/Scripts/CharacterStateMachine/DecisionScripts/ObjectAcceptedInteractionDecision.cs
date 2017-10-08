using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/ObjectAcceptedInteraction")]
public class ObjectAcceptedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.IsInteractionAccepted();
    }
}