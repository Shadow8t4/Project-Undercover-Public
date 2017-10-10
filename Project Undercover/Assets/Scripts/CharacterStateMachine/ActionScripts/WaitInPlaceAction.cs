using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/WaitInPlace")]
public class WaitInPlaceAction : Action {

    public override void StartAct(StateController controller)
    {
        // Debug.Log("Waiting in place!");
        if (!controller.IsInteracting)
            controller.IsInteracting = true;
        controller.Destination = controller.transform.position;
        controller.characterAnimator.SetBool(CharacterAnimator.Params.Interacting, true);
        controller.characterAnimator.SetTrigger(controller.Interactor.SelectedInteraction.objectAnimationTrigger);
    }

    public override void Act(StateController controller)
    {
        controller.FaceInteractor();
    }

    public override void EndAct(StateController controller)
    {
        // Debug.Log("Done waiting in place!");
        if (controller.characterAnimator.GetTrigger(CharacterAnimator.Params.Interrupted))
            controller.Interactor.characterAnimator.SetTrigger(CharacterAnimator.Params.Interrupted);
        controller.Interactor = null;
    }
}
