using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/ObjectInRangeAndAvailable")]
public class ObjectInRangeAndAvailableDecision : Decision
{
    public override bool Decide(StateController controller)
    {

        return controller.InRangeOfSelectedObject() && controller.SelectedObject.Interactor == null;
    }
}

