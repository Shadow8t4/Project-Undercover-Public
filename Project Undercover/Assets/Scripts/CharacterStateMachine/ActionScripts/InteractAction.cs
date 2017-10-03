using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/Interact")]
public class InteractAction : Action
{
    public CharacterAnimator.Params trigger;

    public override void StartAct(StateController controller)
    {
        controller.characterAnimator.SetTrigger(trigger);
        ProgressPanelController.ActivePanel.Reveal("Passing Message...");
    }

    public override void Act(StateController controller)
    {
        AnimatorStateInfo info = controller.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName(CharacterAnimator.GetParamName(trigger)))
            return;
        float progress = info.normalizedTime;
        ProgressPanelController.ActivePanel.Progress = progress;
    }

    public override void EndAct(StateController controller)
    { 
        ProgressPanelController.ActivePanel.Hide();
        controller.SelectedObject = null;
    }
}