using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    /// <summary>
    /// 現在のSceneを外部から、BaseSceneで取得
    /// </summary>
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    public void LoadScene(Define.Scene type)
    {
        // 現在のSceneをクリア
        Managers.Clear();
        // 移動
        SceneManager.LoadScene(GetSceneName(type));
    }

    /// <summary>
    /// EnumのName取得
    /// </summary>
    private string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    /// <summary>
    /// クリア
    /// </summary>
    public void Clear()
    {
        CurrentScene.Clear();
    }

}
