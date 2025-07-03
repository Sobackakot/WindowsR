using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

namespace Drag.Item
{
    public class DraggableItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private SelectionFrame selection;
        public List<DraggableItem> selectedItems = new();
        public List<DraggableItem> draggableItems = new();
        private Canvas canvas;
        private bool isDragging = false;
        private Dictionary<DraggableItem, Vector2> dragOffsets = new();
        private DraggableItem currentDraggableItem; 

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            draggableItems.AddRange(GetComponentsInChildren<DraggableItem>(false));
            selection = GetComponent<SelectionFrame>();
        }
        private void OnEnable()
        { 
            selection.OnAddSelectedItem += AddSelectedItem;
            selection.OnResetSelectedItems += ResetSelectedItems;
        }
        private void OnDisable()
        { 
            selection.OnAddSelectedItem -= AddSelectedItem;
            selection.OnResetSelectedItems -= ResetSelectedItems;
        }
        private void AddSelectedItem(DraggableItem items)
        {
            selectedItems.Add(items);
        }
        private void ResetSelectedItems()
        {
            foreach (var item in selectedItems)
                item.ResetSelectionFrame();
            selectedItems.Clear();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            SetCurrentDraggableItem();
            ResetSelectedItems();
            currentDraggableItem.OnPointerClick(eventData);
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetCurrentDraggableItem();
            BeginDragSingleItem(eventData);
            if (selectedItems.Count > 1 && selectedItems.Contains(currentDraggableItem))
            {
                BeginDragAllSelectedItems(eventData);
                isDragging = true;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            DragSingleItem(eventData); 
            DragAllSelectedItems(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragAllSelectedItems();
            currentDraggableItem?.OnEndDrag(eventData);
            isDragging = false;
        }

        private void EndDragAllSelectedItems()
        {
            foreach (var item in selectedItems)
            {
                item.GetComponent<CanvasGroup>().blocksRaycasts = true;
                item.GetComponent<CanvasGroup>().alpha = 1f;
            }
            dragOffsets.Clear();
        }

        private void DragAllSelectedItems(PointerEventData eventData)
        {
            foreach (var item in selectedItems)
            {
                if (!dragOffsets.ContainsKey(item)) continue;
                item.rectTransform.anchoredPosition = GetPointLocalPosition(eventData, item);
            }
        }

        private void DragSingleItem(PointerEventData eventData)
        {
            if (!isDragging)
            {
                currentDraggableItem?.OnDrag(eventData);
                return;
            }
        }

        
        private void BeginDragAllSelectedItems(PointerEventData eventData)
        {
            dragOffsets.Clear();
            foreach (var item in selectedItems)
            {
                Vector2 offset = (Vector2)item.rectTransform.position - eventData.position;
                dragOffsets[item] = offset;
                item.GetComponent<CanvasGroup>().blocksRaycasts = false;
                item.GetComponent<CanvasGroup>().alpha = 0.6f;
            }
        }

        private void BeginDragSingleItem(PointerEventData eventData)
        {
            if (selectedItems.Count <= 1 || !selectedItems.Contains(currentDraggableItem))
            {
                ResetSelectedItems();
                currentDraggableItem?.OnBeginDrag(eventData);
                currentDraggableItem?.ResetSelectionFrame(); 
            }
        }

        private void SetCurrentDraggableItem()
        {
            currentDraggableItem = null;
            foreach (var item in draggableItems)
            {
                if (item.hasHitPointCursor)
                {
                    currentDraggableItem = item;
                    break;
                }
            }
        }
        private bool GetHasHitPointerCursorOnUI()
        {
            foreach (var item in draggableItems)
                if (item.hasHitPointCursor) return true;
            return false;
        }
        private Vector2 GetPointLocalPosition(PointerEventData eventData, DraggableItem item)
        {
            Vector2 newPos = eventData.position + dragOffsets[item];
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, newPos, eventData.pressEventCamera, out localPoint);
            return localPoint;
        }
       
      
    }
}