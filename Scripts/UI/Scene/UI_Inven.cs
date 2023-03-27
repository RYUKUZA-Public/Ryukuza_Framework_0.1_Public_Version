using UnityEngine;

/// <summary>
/// TODO. インベントリ Test
/// TODO. ポップアップ Test
/// </summary>
public class UI_Inven : UI_Scene
{
    private enum GameObjects
    {
        GridPanel
    }

    private void Start() => Init();

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        base.Init();
        // バインディング
        Bind<GameObject>(typeof(GameObjects));
        // gridPanel 取得
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        // アイテムデータ削除
        foreach (Transform transform in gridPanel.transform)
            Managers.Resource.Destroy(transform.gameObject);

        // TODO. Testアイテム、生成
        // アイテムデータ生成
        for (int i = 0; i < 8; i++)
        {
            // アイテム Prefab 生成
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: gridPanel.transform).gameObject;
            // チェック
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            // アイテムデータセット
            invenItem.SetInfo($"テストアイテム{i}番");
        }
    }
}
