using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public virtual void StartAct(StateController controller)
    {

    }

    public virtual void Act(StateController controller)
    {

    }

    public virtual void EndAct(StateController controller)
    {

    }
}