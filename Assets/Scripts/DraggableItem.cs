using UnityEngine.EventSystems;
using UnityEngine.UI; 
using UnityEngine;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline line;
    public RectTransform rectTransform { get; set; }
    private void Awake()
    {
        line = GetComponent<Outline>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        line.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        line.enabled = false;
    }
}
