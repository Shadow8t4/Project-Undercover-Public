using UnityEngine;
using UnityEngine.EventSystems; //required for Event data

public class CanvasClickCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject currentHover;
    static CanvasClickCheck Singleton;

    void Start()
    {
        if (Singleton != null)
            Debug.LogError("Should only be one Canvas Click Check");
        Singleton = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            currentHover = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentHover = null;
    }

    void Update()
    {
        if (currentHover)
            Debug.Log(currentHover.name + " @ " + Input.mousePosition);
    }
}
