using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : Photon.PunBehaviour
{
    public virtual void Selected()
    {

    }

    public virtual void Deselected()
    {

    }

    public virtual string GetInteractionTitle()
    {
        return "";
    }
}
