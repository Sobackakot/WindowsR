using Drag.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultipleBeginDrag : MonoBehaviour
{
    RegistrySelectableItems registry; 
    private void Awake()
    {
        registry = FindObjectOfType<RegistrySelectableItems>();
    }
    public bool OnMultipleBeginDrag(PointerEventData eventData, DraggableItem currentDraggableItem)
    {
        if (registry.selectedItems.Count > 1 && registry.selectedItems.Contains(currentDraggableItem))
        {
            registry.ResetOffsetItems();
            CalculateOffsetItems(eventData);
            return true;
        }
        else return false;  
    }
    private void CalculateOffsetItems(PointerEventData eventData)
    {
        foreach (var item in registry.selectedItems)
        {
            Vector2 offset = (Vector2)item.rectTransform.position - eventData.position;
            registry.SetOffsetItem(item, offset);
            item.GetComponent<CanvasGroup>().blocksRaycasts = false;
            item.GetComponent<CanvasGroup>().alpha = 0.6f;
        }
    } 
}
