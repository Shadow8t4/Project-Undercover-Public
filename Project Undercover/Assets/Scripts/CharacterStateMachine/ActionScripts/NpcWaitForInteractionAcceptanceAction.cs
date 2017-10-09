using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/NpcWaitForInteractionAcceptance")]
public class NpcWaitForInteractionAcceptanceAction : Action
{

    public override void StartAct(StateController controller)
    {
        controller.InitiateInteractionWithSelectedObject();
        //controller.Destination = controller.SelectedObject.transform.position;
    }
}

