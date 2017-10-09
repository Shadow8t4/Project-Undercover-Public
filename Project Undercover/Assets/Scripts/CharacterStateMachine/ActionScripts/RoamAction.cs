using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/Roam")]
public class RoamAction : Action
{
    public override void StartAct(StateController controller)
    {
        // Debug.Log("Started Roaming");
        controller.StartRoaming();
        controller.characterAnimator.SetBool(CharacterAnimator.Params.Interacting, false);
    }

    public override void Act(StateController controller)
    {
        if (controller.Interactor != null && !controller.IsInteracting && InRangeOfInteractor(controller))
        {
            controller.AcceptInteraction();
        }
    }

    public override void EndAct(StateController controller)
    {
        // Debug.Log("Stopping coroutine");
        controller.StopRoaming();
    }

    private bool InRangeOfInteractor(StateController controller)
    {
        return (controller.Interactor.transform.position - controller.transform.position).magnitude < StateController.INTERACT_RANGE;
    }
}
