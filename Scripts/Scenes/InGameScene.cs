public class InGameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        // Sceneタイプ指定
        SceneType = Define.Scene.InGame;
        // インベントリー UI
        Managers.UI.ShowSceneUI<UI_Inven>();

        var dix = Managers.Data.StatsDict;
    }

    public override void Clear()
    {
        // TODO. クリア
    }
}
