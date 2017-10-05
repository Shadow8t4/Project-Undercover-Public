using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/Roam")]
public class RoamAction : Action
{
    public override void StartAct(StateController controller)
    {
        Debug.Log("Started Roaming");
        controller.StartRoaming();
    }

    public override void Act(StateController controller)
    {
        if (controller.Interactor != null && !controller.IsInteracting)
        {
            Debug.Log("Accepting incomming interaction!");
            controller.AcceptInteraction();
        }
    }

    public override void EndAct(StateController controller)
    {
        Debug.Log("Stopping coroutine");
        controller.StopRoaming();
    }
}
