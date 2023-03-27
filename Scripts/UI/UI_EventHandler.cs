using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    /// <summary>
    /// クリック_コールバック
    /// </summary>
    public Action<PointerEventData> OnPointerClickHandler = null;
    /// <summary>
    /// ドラッグ_コールバック
    /// </summary>
    public Action<PointerEventData> OnDragHandler = null;
    
    public void OnPointerClick(PointerEventData eventData) 
        => OnPointerClickHandler?.Invoke(eventData);

    public void OnDrag(PointerEventData eventData) 
        => OnDragHandler?.Invoke(eventData);
}
