using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/CheckForInterrupts")]
public class CheckForInterruptsAction : Action {

    public override void Act(StateController controller)
    {
        if (Input.GetMouseButtonDown(0))
            controller.characterAnimator.SetTrigger(CharacterAnimator.Params.Interrupted);
    }
}
