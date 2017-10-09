using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/NpcInteract")]
public class NpcInteractAction : Action
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
    }

    public override void EndAct(StateController controller)
    {
        controller.FinishInteraction();
    }
}

