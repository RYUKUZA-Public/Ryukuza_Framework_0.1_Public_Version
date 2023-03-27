using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 製作したいゲームによって、その処理が違う。
/// </summary>
public class InputManager
{
    /// <summary>
    /// Listener Pattern
    /// 入力をチェックした後、入力がある場合は伝播する。
    /// </summary>
    public Action KeyAction = null;
    /// <summary>
    /// マウス_イベント
    /// </summary>
    public Action<Define.MouseEvent> MouseAction = null;

    private bool _pressed = false;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // キー入力があれば伝播
        if (Input.anyKey && KeyAction != null)
            KeyAction?.Invoke();

        if (MouseAction != null)
        {
            // 左マウスクリック
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                
                _pressed = false;
            }
        }
    }
    
    /// <summary>
    /// クリア
    /// </summary>
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}