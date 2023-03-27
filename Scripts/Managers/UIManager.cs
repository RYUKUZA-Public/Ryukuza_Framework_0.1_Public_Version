using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // キャンバスのSortOrder管理（ポップアップ管理）
    private int _order = 10;
    
    // ポップアップは、一番最後のキャンバスから、削除されるので、Stack
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    /// <summary>
    /// ポップアップが、オンになるとキャンバスのSortOrderを管理
    /// </summary>
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        // ポップアップと関連のない一般UI
        else
        {
            canvas.sortingOrder = 0;
        }

    }
    
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        
        if (parent != null)
            go.transform.SetParent(parent);
        
        return go.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI = sceneUI;
        
        go.transform.SetParent(Root.transform);
        
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);
        
        go.transform.SetParent(Root.transform);
        
        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("ポップアップを閉じる失敗");
            return;
        }

        ClosePopupUI();
    }
    
    public void ClosePopupUI(string name = null)
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;

    }

    public void CloseAllPopupUI(string name = null)
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    /// <summary>
    /// クリア
    /// </summary>
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
