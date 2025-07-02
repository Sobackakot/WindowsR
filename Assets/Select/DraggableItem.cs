 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Drag.Item
{
    public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    { 
        public bool hasHitPointCursor { get; private set; }
        public Transform m_Transform { get; set; }
        public RectTransform rectTransform { get; set; }
        private CanvasGroup canvasGroup;
        private Canvas canvas;
        private Outline line;
        public bool isPointerEnter { get; private set; }
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
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            hasHitPointCursor = false;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            rectTransform.SetParent(m_Transform);
        }
        public void PickEnterItem()
        {
            line.enabled = true;
            isPointerEnter = true;
        }
        public void PickEndItem()
        {
            line.enabled = false;
            isPointerEnter = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        { 
            PickEnterItem();
            hasHitPointCursor = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        { 
            PickEndItem();
            hasHitPointCursor = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        { 
            hasHitPointCursor = true; 
        }
    }
}

