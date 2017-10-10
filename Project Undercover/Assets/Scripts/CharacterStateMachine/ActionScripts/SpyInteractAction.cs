using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/SpyInteract")]
public class SpyInteractAction : Action
{
    public override void StartAct(StateController controller)
    {
        controller.characterAnimator.SetTrigger(controller.SelectedInteraction.initiatorAnimationTrigger);
        controller.characterAnimator.SetBool(CharacterAnimator.Params.Interacting, true);
        controller.MoveForSelectedObjectInteraction();
    }

    public override void Act(StateController controller)
    {
        controller.FaceSelectedObject();
        AnimatorStateInfo info = controller.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(CharacterAnimator.GetParamName(controller.SelectedInteraction.initiatorAnimationTrigger)))
        {
            float progress = info.normalizedTime;
            ProgressPanelController.ActivePanel.Progress = progress;
        }
    }

    public override void EndAct(StateController controller)
    {
        // Debug.Log("Ending SpyInteract");
        ProgressPanelController.ActivePanel.Hide();
        controller.FinishInteraction();
    }
}