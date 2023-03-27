using UnityEngine;

/// <summary>
/// ユチル関数
/// </summary>
public class Util
{
    /// <summary>
    /// コンポーネントを見つけるか、ない場合は追加
    /// </summary>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    /// <summary>
    /// 子供オブジェクト探し
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // すべてのGameObjectには、Transformが存在するため、Transformを使用すると
        // 既存の「FindChild」を再利用することができる。
        Transform transform = FindChild<Transform>(go, name, recursive);
        // nullチェック後に返す
        return transform == null ? null : transform.gameObject;
    }

    /// <summary>
    /// 子供オブジェクト探し
    /// </summary>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        // 子供のみ探す
        if (recursive == false)
        {
            // 子供の数だけ
            for (int i = 0; i < go.transform.childCount; i++)
            {
                // 子供を探して
                Transform transform = go.transform.GetChild(i);
                // nameは null であるかもしれない、名前が同じなら
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    // コンポーネントを取得して配布
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        // 子供の子供まで、探す
        else
        {
            // すべての子供
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // nameは null であるかもしれない、名前が同じなら
                // コンポーネントを取得して配布
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
