using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/IsInteracting")]
public class IsInteractingDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.IsInteracting;
    }
}