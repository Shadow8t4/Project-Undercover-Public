using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : SelectableObject {

    protected override void Update()
    {
        base.Update();
        if (Interactor != null && !IsInteracting)
        {
            AcceptInteraction();
        }
    }
}
