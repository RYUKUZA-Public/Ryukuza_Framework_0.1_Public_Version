using UnityEngine;

/// <summary>
/// マネージャー統合
/// </summary>
public class Managers : MonoBehaviour
{
    // 唯一性保障
    private static Managers s_instance;
    // 傘下の各Managerを返す形であるため、private
    private static Managers Instance { get { Init(); return s_instance; } }

    // Managerを追加時、こちらに
    #region [Core]
    private DataManager _data = new DataManager();
    private InputManager _input = new InputManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    private void Start() => Init();

    /// <summary>
    /// Managersから代表にUpdate
    /// </summary>
    private void Update() => _input.OnUpdate();

    /// <summary>
    /// 初期化
    /// </summary>
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
        
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
    
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            
            // 必要に応じて各Managerの初期化
            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }

    /// <summary>
    /// シーン移動時に処理すべきものをここで一括処理する。
    /// </summary>
    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
