using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "CharacterStateMachine/Actions/IdleClick")]
public class IdleClickAction : Action
{
    public GameObject wayPointPrefab;

    public override void StartAct(StateController controller)
    {
        ProgressPanelController.ActivePanel.Hide();
        controller.SelectedInteraction = null;
        controller.Interactor = null;
        controller.SelectedObject = null;
        controller.characterAnimator.SetBool(CharacterAnimator.Params.Interacting, false);
    }

    public override void Act(StateController controller)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (controller.Interactor)
            {
                controller.Interactor = null;
                InteractionPanelController.ActivePanel.Hide();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Check first if the player clicked on a selectable object
                SelectableObject selectableObject;
                if (RaycastForSelectableObject(controller, ray, out selectableObject))
                {
                    controller.SelectedObject = selectableObject;
                    return;
                }

                // At this point, the player didn't click on a selectable object,
                // so the player is probably issuing a move command.
                controller.SelectedObject = null;
                RaycastToMoveController(controller, ray);
            }
        }

        // If the player has selected an object, move the player toward that object
        if (controller.SelectedObject && !controller.IsInteracting)
            controller.Destination = controller.SelectedObject.transform.position;
    }

    // Check if StateController clicked on a Selectable Object
    bool RaycastForSelectableObject(StateController controller, Ray ray, out SelectableObject selectableObject)
    {
        int layerMask = LayerMask.NameToLayer("SelectableObject");
        int mask = 1 << layerMask;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            SelectableObject selectable = hit.collider.gameObject.GetComponentInParent<SelectableObject>();
            if (selectable != null && selectable != controller && selectable.HasInteractions())
            {
                selectableObject = selectable;
                return true;
            }
        }
        selectableObject = null;
        return false;
    }

    // Check if StateController clicked on the floor to issue a move command
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
                Instantiate(wayPointPrefab, navhit.position, Quaternion.identity);
                return true;
            }
        }
        return false;
    }
}
