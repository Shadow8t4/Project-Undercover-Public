using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/WaitInPlace")]
public class WaitInPlaceAction : Action {

    public override void StartAct(StateController controller)
    {
        Debug.Log("Waiting in place!");
        controller.FaceInteractor();
        controller.Destination = controller.transform.position;
    }

    public override void EndAct(StateController controller)
    {
        Debug.Log("Done waiting in place!");
    }

}
