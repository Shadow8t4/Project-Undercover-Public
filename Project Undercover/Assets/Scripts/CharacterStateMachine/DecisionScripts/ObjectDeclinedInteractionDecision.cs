﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Decisions/ObjectDeclinedInteraction")]
public class ObjectDeclinedInteractionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.IsInteractionRejected();
    }
}

