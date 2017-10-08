using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/YouAcceptedInteraction")]
class YouAcceptedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (Input.GetKeyDown(KeyCode.E))
            return controller.Interactor != null;
        return false;
    }
}

