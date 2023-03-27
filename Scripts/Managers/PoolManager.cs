using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region [Pool]
    /// <summary>
    /// Pool
    /// </summary>
    class Pool
    {
        /// <summary>
        /// オリジナルデータ
        /// </summary>
        public GameObject Original { get; private set; }
        /// <summary>
        /// 親になるRoot
        /// </summary>
        public Transform Root { get; set; }
        /// <summary>
        /// Poolを入れるスタック
        /// </summary>
        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            // 整理
            // ルートで使用するオブジェクトを作成
            Root = new GameObject().transform;
            // データに応じて、名前を設定
            Root.name = $"{original.name}_Root";
            // カウント分、生成後Push
            for (int i = 0; i < count; i++)
                Push(Create());
        }

        /// <summary>
        /// 生成
        /// </summary>
        public Poolable Create()
        {
            // オブジェクト生成 (クローン)
            GameObject go = Object.Instantiate<GameObject>(Original);
            // 名前の定理
            go.name = Original.name;
            // Poolable形式でリターン
            return go.GetOrAddComponent<Poolable>();
        }
        
        /// <summary>
        /// Push返還
        /// Stackに、追加および返却時に必要な作業
        /// </summary>
        public void Push (Poolable poolable)
        {
            if (poolable == null)
                return;
            
            // 非活性化状態
            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;
            // スタックに追加
            _poolStack.Push(poolable);
        }

        /// <summary>
        /// Pop 取り出し
        /// </summary>
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            
            // 一つでも待機中
            if (_poolStack.Count > 0)
                // 取り出し
                poolable = _poolStack.Pop();
            // なければ
            else
                // 生成
                poolable = Create();
            
            // 活性化状態
            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 解除用
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            
            poolable.transform.parent = parent;
            poolable.isUsing = true;
            
            return poolable;
        }
    }
    #endregion
    
    // PoolManagerは、様々なpoolを持っている。
    // そのPoolリストをディクショナリーで、管理
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private Transform _root;
    
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    /// <summary>
    /// Pool 生成
    /// </summary>
    private void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;
        
        // ディクショナリーに追加
        _pools.Add(original.name, pool);
    }

    /// <summary>
    /// 返還
    /// </summary>
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pools.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pools[name].Push(poolable);
    }
    
    /// <summary>
    /// 取り出し
    /// </summary>
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        // 初めて作る場合、データがないため、チェック後に、生成が必要
        if (_pools.ContainsKey(original.name) == false)
            CreatePool(original);
        
        return _pools[original.name].Pop(parent);
    }

    /// <summary>
    /// 原本取得
    /// </summary>
    public GameObject GetOriginal(string name)
    {
        if (_pools.ContainsKey(name) == false)
            return null;
        
        return _pools[name].Original;
    }

    /// <summary>
    /// クリア
    /// </summary>
    public void Clear()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        _pools.Clear();
    }
}
