using UnityEngine;

public class ResourceManager
{
    /// <summary>
    /// ロード
    /// </summary> 
    public T Load<T>(string path) where T : Object
    {
        // Prefabの場合、Poolで探してからそれを返却
        if (typeof(T) == typeof(GameObject))
        {
            // 経路が名前で、入るため加工が必要
            string name = path;
            // さいご「/」を見つけた後
            int index = name.LastIndexOf('/');
            // /「/」以降の文字をすべて保存
            if (index >= 0)
                name = name.Substring(index + 1);

            // すぐ取得
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        
        // なければロード
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 生成
    /// </summary> 
    public GameObject Instantiate(string path, Transform parent = null)
    {
        // ロードPrefabの場合、Poolで探してからそれを返却
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            Debug.Log($"Prefab ロード失敗 {path}");
            return null;
        }
        
        // Poolingされたオブジェクトがある場合は、再利用
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;
        
        // Pooling対象でない場合
        GameObject go = Object.Instantiate(original, parent);
        // (Clone) テキスト削除
        go.name = original.name;
        return go;
    }

    /// <summary>
    /// 破壊
    /// </summary> 
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        // Pooling対象の場合、Poolに返す。
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }
        
        // Pooling対象でない場合は、破壊
        Object.Destroy(go);
    }
}
