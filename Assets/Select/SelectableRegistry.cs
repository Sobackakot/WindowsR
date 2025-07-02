using System.Collections.Generic;
using UnityEngine;

namespace Drag.Item
{
    public class SelectableRegistry : MonoBehaviour
    {
        public readonly List<DraggableItem> itemFromScreen = new();
        private void Awake()
        {
            itemFromScreen.AddRange(GetComponentsInChildren<DraggableItem>(false)); 
        }
    }
}