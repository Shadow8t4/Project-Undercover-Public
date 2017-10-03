using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/WaitForAnimation")]
public class WaitForTransitionDecision : Decision
{
    public string fromState, toState;

    public override bool Decide(StateController controller)
    {
        string transitionName = fromState + " -> " + toState;
        var currentTransition = controller.animator.GetAnimatorTransitionInfo(0);
        if (currentTransition.IsName(transitionName))
        {
            return true;
        }
        return false;
    }
}