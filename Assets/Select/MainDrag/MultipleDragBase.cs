using Drag.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MultipleDragBase  : MonoBehaviour
{
    public virtual bool OnBeginDrag(PointerEventData eventData, DraggableItem currentDraggableItem) 
    {
        return true; 
    }
    public virtual void OnDrag(PointerEventData eventData) { }
    public virtual void OnEndDrag(PointerEventData eventData) { }
}
