using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/Roam")]
public class RoamAction : Action
{
    private float roomSize = 10.0f;

    public override void StartAct(StateController controller)
    {
        controller.StartCoroutine(changeDestination(controller));
    }

    public override void EndAct(StateController controller)
    {
        controller.StopCoroutine(changeDestination(controller));
    }

    private IEnumerator changeDestination(StateController controller)
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.value * 10.0f);
            controller.Destination = GetRandomLocation();
        }
    }

    private Vector3 GetRandomLocation()
    {
        var randTarget = new Vector3(roomSize - (roomSize * 2 * Random.value), 0.0f, roomSize - (roomSize * 2 * Random.value));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randTarget, out hit, 1.0f, NavMesh.AllAreas))
            return hit.position;
        else
            return GetRandomLocation();
    }
}
