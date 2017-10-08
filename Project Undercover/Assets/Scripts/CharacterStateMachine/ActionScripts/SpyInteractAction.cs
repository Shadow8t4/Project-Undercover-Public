using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/SpyInteract")]
public class SpyInteractAction : Action
{
    public override void StartAct(StateController controller)
    {
        controller.characterAnimator.SetTrigger(controller.SelectedInteraction.characterInteraction);
        Vector3 awayDirection =(controller.transform.position - controller.SelectedObject.transform.position).normalized;
        Vector3 newPos = controller.SelectedObject.transform.position + awayDirection * controller.SelectedInteraction.interactionDistance;
        controller.navMeshAgent.stoppingDistance = 0.0f;
        controller.Destination = newPos;
    }

    public override void Act(StateController controller)
    {
        controller.FaceSelectedObject();
        AnimatorStateInfo info = controller.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(CharacterAnimator.GetParamName(controller.SelectedInteraction.characterInteraction)))
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