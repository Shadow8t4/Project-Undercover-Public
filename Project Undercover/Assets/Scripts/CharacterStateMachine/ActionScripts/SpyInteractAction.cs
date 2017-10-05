using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/SpyInteract")]
public class SpyInteractAction : Action
{
    public override void StartAct(StateController controller)
    {
        controller.FaceSelectedObject();
        controller.characterAnimator.SetTrigger(controller.SelectedInteraction.characterInteraction);
    }

    public override void Act(StateController controller)
    {
        AnimatorStateInfo info = controller.animator.GetCurrentAnimatorStateInfo(0);
        float progress = info.normalizedTime;
        ProgressPanelController.ActivePanel.Progress = progress;
    }

    public override void EndAct(StateController controller)
    {
        Debug.Log("Ending SpyInteract");
        ProgressPanelController.ActivePanel.Hide();
        controller.FinishInteraction();
    }
}