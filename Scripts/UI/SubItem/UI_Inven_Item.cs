using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    private enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    /// <summary>
    /// アイテム名
    /// </summary>
    private string _name;
    
    private void Start() => Init();

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        // バインディング
        Bind<GameObject>(typeof(GameObjects));
        // ItemNameText 取得
        GameObject item = Get<GameObject>((int)GameObjects.ItemNameText);
        // テキスト変更
        item.GetComponent<Text>().text = _name;
        // クリックイベント登録 (ボタン)
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((data) =>
        {
            Debug.Log($"{item.GetComponent<Text>().text} アイテムをクリック");
        });
    }
    
    /// <summary>
    /// データセット
    /// </summary>
    public void SetInfo(string name)
    {
        _name = name;
    }
}
