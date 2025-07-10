using Drag.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SingleDragBase : MonoBehaviour
{
    public virtual void OnSingleBeginDrag(PointerEventData eventData, DraggableItem currentDraggableItem) { }
    public virtual void OnSingleDrag(PointerEventData eventData, DraggableItem currentDraggableItem) { }
    public virtual void OnSingleEndDrag(PointerEventData eventData, DraggableItem currentDraggableItem) { }
}
