using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableRegistry : MonoBehaviour
{
    public readonly List<DraggableItem> items = new();
    private void Awake()
    {
        items.AddRange(GetComponentsInChildren<DraggableItem>(false));
    }
}
