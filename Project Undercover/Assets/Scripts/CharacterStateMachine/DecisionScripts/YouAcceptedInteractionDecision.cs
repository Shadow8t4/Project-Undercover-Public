using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/YouAcceptedInteraction")]
public class YouAcceptedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.SelectedInteraction != null && controller.Interactor != null;
    }
}

