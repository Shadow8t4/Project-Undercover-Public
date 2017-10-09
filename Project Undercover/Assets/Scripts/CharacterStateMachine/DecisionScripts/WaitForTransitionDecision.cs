using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/WaitForTransition")]
public class WaitForTransitionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        string fromState = CharacterAnimator.GetParamName(controller.SelectedInteraction.characterInteraction);
        string toState = CharacterAnimator.GetParamName(CharacterAnimator.Params.Idle);
        string transitionName = fromState + " -> " + toState;
        var currentTransition = controller.animator.GetAnimatorTransitionInfo(0);
        if (currentTransition.IsName(transitionName))
        {
            controller.SelectedInteraction.ExecuteResult(controller);
            return true;
        }
        return false;
    }
}