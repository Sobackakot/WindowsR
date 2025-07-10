 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Drag.Item
{
    public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {  
        public Transform m_Transform { get; set; }
        public RectTransform rectTransform { get; set; }
        private CanvasGroup canvasGroup;
        private Canvas canvas;
        private Outline line;
        public bool hasHitPointCursor { get; private set; } 
        public bool isDraggableItem { get; private set; } 
        public bool inSelectionFrame { get; private set; }
        private void Awake()
        {
            m_Transform = GetComponent<Transform>();
            rectTransform = GetComponent<RectTransform>();
            canvas = m_Transform.GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            line = GetComponent<Outline>(); 
        }
        private void OnEnable()
        {
            line.enabled = false;
            hasHitPointCursor = false; 
        }
        private void OnDisable()
        {
            line.enabled = false; 
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("BeginDrag " + name);
            isDraggableItem = true;
            hasHitPointCursor = true;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
            m_Transform = transform.parent;
            rectTransform.SetParent(canvas.transform);
            rectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            hasHitPointCursor = true;
            isDraggableItem = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("EndDrag " + name);
            isDraggableItem = false;
            hasHitPointCursor = false;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            rectTransform.SetParent(m_Transform);
        }
        public void SetInSelectionFrame()
        {
            inSelectionFrame = true;
            LineEnable();
        }
        public void ResetItem()
        {
            inSelectionFrame = false;
            LineDisable();
        }
        public void LineEnable()
        {
            line.enabled = true; 
        }
        public void LineDisable()
        {
            if (!inSelectionFrame)
                line.enabled = false; 
        }
        public void PointerEnter()
        { 
            hasHitPointCursor = true;
        }
        public void PointerExit()
        { 
            hasHitPointCursor = false;
        }
        public void OnPointerEnter(PointerEventData eventData)
        { 
            LineEnable();
            PointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        { 
            LineDisable();
            PointerExit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEnter();
            LineEnable();
        }
    }
}

