using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Drag.Item
{
    public class SelectionFrame : MonoBehaviour
    { 
        public event Action<DraggableItem> OnAddSelectedItem; 
        public event Action OnResetSelectedItems;


        SelectableRegistry registry;
        public GUISkin GUISkin; // GUI skin for the selection box.
        private Rect screenSpaceRect; // Rectangle representing the selection box. 

        private Vector2 startPoint; // Starting point of the selection box.
        private Vector2 endPoint; // Ending point of the selection box.
        private int sortingLayer = 99; // Sorting layer for the GUI.
        private float minDragDistance = 10f; // Minimum distance to start drawing the frame. 
        public bool onSelectionStay { get; private set; }


        private void OnEnable()
        {
            OnResetSelectedItems?.Invoke();
        }
        private void Awake()
        {
            registry = GetComponent<SelectableRegistry>();
        }

        private void OnGUI()
        { 
            GUI.skin = GUISkin;
            GUI.depth = sortingLayer;
            SelectionStart();
            SelectionStay();

        }
        public void Update()
        { 
            SelectionEnd();
        }
        private void SelectionStart()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = Input.mousePosition; 
                if (IsPointerOverAnyDraggable()) return; 
                onSelectionStay = true;
                OnResetSelectedItems?.Invoke(); 
            }
        }
        private void SelectionStay()
        {
            if (Input.GetMouseButton(0) && onSelectionStay)
            { 
                endPoint = Input.mousePosition;
                if (Vector2.Distance(startPoint, endPoint) < minDragDistance) return;   
                screenSpaceRect = GetRectFrame(startPoint, endPoint);
                DrawFrame(screenSpaceRect); 
            }
        }

        private void SelectionEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                endPoint = Input.mousePosition; 
                SelectionFrameFromScreen(screenSpaceRect);
                onSelectionStay = false; 
            }
        }
        //Вычисляет корректный прямоугольник
        private Rect GetRectFrame(Vector2 startPoint, Vector2 endPoint)
        {
            float posX = Mathf.Min(startPoint.x, endPoint.x);
            float posY = Mathf.Min(startPoint.y, endPoint.y);
            float widthX = Mathf.Abs(endPoint.x - startPoint.x);
            float heightY = Mathf.Abs(endPoint.y - startPoint.y);

            return new Rect(posX, posY, widthX, heightY);
        }

        //Отрисовывает GUI.Box — прямоугольник рамки выбора.
        private void DrawFrame(Rect screenRect)
        {
            //Unity GUI использует экранные координаты снизу вверх, а в Screen Space Y идёт вниз
            //Поэтому y координата корректируется: Screen.height - y - height.

            float x = screenRect.x;
            float y = Screen.height - screenRect.y - screenRect.height;
            float width = screenRect.width;
            float height = screenRect.height;

            Rect newBox = new Rect(x, y, width, height);
            GUI.Box(newBox, "");
        }

        private void SelectionFrameFromScreen(Rect screen)
        {
            foreach (var item in registry.itemFromScreen)
            {
                Rect itemRect = GetRectItem(item);
                //проверяет, пересекаются ли прямоугольники
                if (screen.Overlaps(itemRect, true))
                {
                    item.SetInSelectionFrame();
                    item.LineEnable();
                    OnAddSelectedItem?.Invoke(item);
                    screenSpaceRect = Rect.zero;
                }
            }
        }

        //Вычисляет прямоугольник (DraggableItem item) - UI-объекта на экране
        private Rect GetRectItem(DraggableItem item)
        {
            Vector3[] positions = new Vector3[4];
            item.rectTransform.GetWorldCorners(positions);//возвращает 4 угла RectTransform в мировых координатах.

            //мы получаем координаты в экранном пространстве.
            Vector2 startPoint = RectTransformUtility.WorldToScreenPoint(null, positions[0]);
            Vector2 endPoint = RectTransformUtility.WorldToScreenPoint(null, positions[2]);

            float x = Mathf.Min(startPoint.x, endPoint.x);
            float y = Mathf.Min(startPoint.y, endPoint.y);
            float width = Mathf.Abs(endPoint.x - startPoint.x);
            float height = Mathf.Abs(endPoint.y - startPoint.y);

            return new Rect(x, y, width, height);
        }
        private bool IsPointerOverAnyDraggable()
        {
            foreach (var hit in GetRaycastHitResults())
                if (hit.gameObject.GetComponent<DraggableItem>()) return true;
            return false;
        }
        private List<RaycastResult> GetRaycastHitResults()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> hitResults = new();
            EventSystem.current.RaycastAll(pointerData, hitResults);
            return hitResults;
        } 
    }
}

 