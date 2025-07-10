using Drag.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleEndDrag : MonoBehaviour
{
    RegistrySelectableItems registry;
    private void Awake()
    {
        registry = FindObjectOfType<RegistrySelectableItems>();
    }
    public void OnSingleEndDrag(PointerEventData eventData, DraggableItem currentDraggableItem)
    {
        currentDraggableItem?.OnEndDrag(eventData);
    }
}
