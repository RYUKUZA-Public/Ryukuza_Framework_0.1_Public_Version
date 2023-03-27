using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;
    }

    public override void Clear()
    {
        Debug.Log("ログイン Scene Clear");
    }

    /// <summary>
    /// TODO. Test
    /// </summary>
    public void Update()
    {
        // Scene移動
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Define.Scene.InGame);
            //SceneManager.LoadSceneAsync("InGame");
        }
    }
}
