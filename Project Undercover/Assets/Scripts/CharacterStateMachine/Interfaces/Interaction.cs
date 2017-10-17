using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStateMachine/Interaction")]
public class Interaction : ScriptableObject
{
    // Animation performed by the character
    [Tooltip("Descriptions follow the prompt \"Press 'E' to ...\"")]
    public string interactionDescription;
    [Tooltip("Descriptions follow the prompt \"Press 'E' to ...\"")]
    public string receiverDescription;
    public CharacterAnimator.Params initiatorAnimationTrigger, objectAnimationTrigger;
    public InteractionResult result;
    public float initialRotation;
    public float objectInitialRotation;
    public float interactionDistance = 1.0f;

    public enum InteractionResult
    {
        Nothing, SpyMissionComplete
    }
    
    public int GetHash()
    {
        return interactionDescription.Length + (int)initiatorAnimationTrigger + (int)objectAnimationTrigger;
    }

    public bool CompareHash(int hash)
    {
        return hash == GetHash();
    }

    public void ExecuteResult(StateController controller)
    {
        switch(result)
        {
            case InteractionResult.Nothing:
                break;
            case InteractionResult.SpyMissionComplete:
                SpyMissionComplete(controller);
                break;
            default:
                Debug.LogError("Invalid result selected for execution");
                break;
        }
    }

    //--------------------------------- Result functions ---------------------------------
    void SpyMissionComplete(StateController controller)
    {
        ScorePanelController.CompleteMission();
    }
}
