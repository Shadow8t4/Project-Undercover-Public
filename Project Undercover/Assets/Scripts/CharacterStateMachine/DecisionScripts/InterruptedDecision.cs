using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/Interrupted")]
public class InterruptedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.characterAnimator.GetTrigger(CharacterAnimator.Params.Interrupted))
        {
            Debug.Log("Interrupted");
            return true;
        }
        return false;
    }
}
