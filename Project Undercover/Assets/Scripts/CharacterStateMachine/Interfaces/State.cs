using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/State")]
public class State : ScriptableObject
{

    public Action[] actions;
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void DoStartActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].StartAct(controller);
        }
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    public void DoEndActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].EndAct(controller);
        }
    }

    private void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller) == transitions[i].transitionValue;

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
                break;
            }
        }
    }


}