using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/NpcApproachingTarget")]
public class NpcApproachingTargetAction : Action
{
    public override void StartAct(StateController controller)
    {
        //Debug.Log("Approaching object");
    }

    public override void Act(StateController controller)
    {
        controller.Destination = controller.SelectedObject.transform.position;
    }

    public override void EndAct(StateController controller)
    {
        //Debug.Log("No longer approaching object");
    }
}