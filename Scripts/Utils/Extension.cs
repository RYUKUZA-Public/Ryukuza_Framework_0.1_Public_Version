using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Extension機能
/// </summary>
public static class Extension
{
    /// <summary>
    /// コンポーネントを見つけるか、ない場合は追加
    /// </summary>
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    
    /// <summary>
    /// UIイベントを追加(クリーグ、ドラッグ、...)
    /// </summary>
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
}
