using UnityEngine;
using UnityEngine.EventSystems;

namespace Drag.Item
{
    public class MainDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private RegistrySelectableItems registry; 

        private MultipleBeginDrag multipleBeginDrag;
        private MultipleDrag multipleDrag;
        private MultipleEndDrag multipleEndDrag;

        private SingleBeginDrag singleBeginDrag;
        private SingleDrag singleDrag;
        private SingleEndDrag singleEndDrag;

        private bool isDragging = false;
        private void Awake()
        {
            registry = GetComponent<RegistrySelectableItems>();

            multipleBeginDrag = GetComponent<MultipleBeginDrag>();
            multipleDrag = GetComponent<MultipleDrag>();
            multipleEndDrag = GetComponent<MultipleEndDrag>();

            singleBeginDrag = GetComponent<SingleBeginDrag>();
            singleDrag = GetComponent<SingleDrag>();
            singleEndDrag = GetComponent<SingleEndDrag>(); 

        } 

        public void OnBeginDrag(PointerEventData eventData)
        { 
            registry.SetCurrentDraggableItem();
            singleBeginDrag.OnSingleBeginDrag(eventData, registry.currentDraggableItem); 
            isDragging = multipleBeginDrag.OnMultipleBeginDrag(eventData, registry.currentDraggableItem); 
        }

        public void OnDrag(PointerEventData eventData)
        { 
            if(!isDragging)
                singleDrag.OnSingleDrag(eventData, registry.currentDraggableItem);
            multipleDrag.OnMultipleDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            multipleEndDrag.OnMultipleEndDrag(eventData);
            singleEndDrag.OnSingleEndDrag(eventData, registry.currentDraggableItem);
            isDragging = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            registry.SetCurrentDraggableItem();
            registry.ResetItems();
            registry.currentDraggableItem.OnPointerClick(eventData);
        }
         
      
    }
}