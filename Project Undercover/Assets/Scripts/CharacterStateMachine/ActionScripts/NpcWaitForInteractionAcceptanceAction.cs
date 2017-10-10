using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/NpcWaitForInteractionAcceptance")]
public class NpcWaitForInteractionAcceptanceAction : Action
{

    public override void StartAct(StateController controller)
    {
        controller.InitiateInteractionWithSelectedObject();
    }
}

