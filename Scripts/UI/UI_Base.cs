using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
    /// <summary>
    /// どんなタイプなのかわからないので、TextやButtonなどの最上位の親を使用
    /// タイプを入れると、そのタイプのリストを取得
    /// </summary>
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, Object[]>();

    public abstract void Init();
    
    /// <summary>
    /// Reflectionを利用して、Typeにenumを渡す
    /// </summary>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // タイプの名前を取得
        string[] names = Enum.GetNames(type);
        // 取得した数だけ配列作成
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        // ディクショナリーに追加
        _objects.Add(typeof(T), objects);
        
        // 該当する子供たちを探して取得
        for (int i = 0; i < names.Length; i++)
        {
            // GameObject
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            // component
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"ロード失敗 : {names[i]}");
        }
    }

    /// <summary>
    /// ディショナリーからコンポーネント取得
    /// </summary>
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        // 該当タイプ(Button、Textなど)を入れて値があるかどうかをチェック
        // 値がなければ、null
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;
        
        // 値があれば取得した、配列のidxを探して配布
        // ex) (int)Texts.ScoreText <- ScoreTextのインデックス
        return objects[idx] as T;
    }
    
    protected Text GetText(int idx) => Get<Text>(idx);
    protected Button GetButton(int idx) => Get<Button>(idx);
    // TODO. まだ Images enumがない
    protected Image GetImage(int idx) => Get<Image>(idx);
    protected GameObject GetObject(int idx) => Get<GameObject>(idx);

    /// <summary>
    /// UIイベントを追加(クリック、ドラッグ、...)
    /// </summary>
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // UI_EventHandlerを探すかなければ追加
        UI_EventHandler evt = go.GetOrAddComponent<UI_EventHandler>();

        // イベント分岐
        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnPointerClickHandler -= action;
                evt.OnPointerClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
