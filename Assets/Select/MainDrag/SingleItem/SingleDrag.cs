using Drag.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleDrag : MonoBehaviour
{
    RegistrySelectableItems registry;
    private void Awake()
    {
        registry = FindObjectOfType<RegistrySelectableItems>();
    }
    public void OnSingleDrag(PointerEventData eventData, DraggableItem currentDraggableItem)
    {
        currentDraggableItem?.OnDrag(eventData);
        return;
    }
}
