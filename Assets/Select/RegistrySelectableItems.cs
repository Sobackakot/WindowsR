using System.Collections.Generic;
using UnityEngine;

namespace Drag.Item
{
    public class RegistrySelectableItems : MonoBehaviour
    { 
        public readonly List<DraggableItem> selectedItems = new();
        public readonly List<DraggableItem> draggableItems = new();
        public readonly Dictionary<DraggableItem, Vector2> itemsOffset = new();
        
        public  DraggableItem currentDraggableItem { get; private set; }

        private SelectionFrame selection;

        private void Awake()
        {
            selection = GetComponent<SelectionFrame>();
            draggableItems.AddRange(GetComponentsInChildren<DraggableItem>(false));  
        }
        private void OnEnable()
        {
            selection.OnAddSelectedItem += SetItem;
            selection.OnResetSelectedItems += ResetItems;
        }
        private void OnDisable()
        {
            selection.OnAddSelectedItem -= SetItem;
            selection.OnResetSelectedItems -= ResetItems;
        }
        public void SetCurrentDraggableItem()
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
        public void SetItem(DraggableItem items)
        {
            selectedItems.Add(items);
        }
        public void ResetItems()
        {
            foreach (var item in selectedItems)
                item.ResetItem();
            selectedItems.Clear();
        }

        public void SetOffsetItem(DraggableItem item, Vector2 offset)
        {
            itemsOffset[item] = offset;
        }
        public void ResetOffsetItems()
        {
            itemsOffset.Clear();
        }
        public Vector2 GetOffsetItem(DraggableItem item)
        {
            return itemsOffset[item];
        }
    }
}