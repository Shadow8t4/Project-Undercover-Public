using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/IdleClick")]
public class IdleClickAction : Action
{
    public override void Act(StateController controller)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            SelectableObject selectableObject;
            if (RaycastForSelectableObject(controller, ray, out selectableObject))
            {
                controller.SelectedObject = selectableObject;
                controller.Destination = selectableObject.gameObject.transform.position;
                return;
            }

            controller.SelectedObject = null;
            RaycastToMoveController(controller, ray);
        }
    }

    // See if StateController clicked on a Selectable Object
    bool RaycastForSelectableObject(StateController controller, Ray ray, out SelectableObject selectableObject)
    {
        int layerMask = LayerMask.NameToLayer("SelectableObject");
        int mask = 1 << layerMask;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            SelectableObject selectable = hit.collider.gameObject.GetComponentInParent<SelectableObject>();
            if (selectable != null && (StateController)selectable != controller)
            {
                selectableObject = selectable;
                return true;
            }
        }
        selectableObject = null;
        return false;
    }

    // See if StateController clicked on the floor to issue a move command
    bool RaycastToMoveController(StateController controller, Ray ray)
    {
        int layerMask = LayerMask.NameToLayer("Floor");
        int mask = 1 << layerMask;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            NavMeshHit navhit;
            if (NavMesh.SamplePosition(hit.point, out navhit, 1.0f, NavMesh.AllAreas))
            {
                controller.Destination = navhit.position;
                Instantiate(controller.wayPointPrefab, navhit.position, Quaternion.identity);
                return true;
            }
        }
        return false;
    }
}
