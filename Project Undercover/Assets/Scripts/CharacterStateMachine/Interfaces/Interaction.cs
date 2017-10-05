using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Interaction")]
public class Interaction : ScriptableObject
{
    // Animation performed by the character
    public string interactionDescription;
    public CharacterAnimator.Params characterInteraction;
    public InteractionResult result;

    public enum InteractionResult
    {
        Nothing, SpyMissionComplete
    }
}
