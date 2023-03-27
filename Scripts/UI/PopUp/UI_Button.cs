using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    
    /// <summary>
    /// Enumの名前は、Bindのため、
    /// オブジェクトの名前と同一でなければならない。
    /// </summary>
    enum Buttons
    {
        PointButton
    }
    enum Texts
    {
        PointText,
        ScoreText
    }
    enum GameObjects
    {
        TestObject
    }
    enum Images
    {
        TestImage
    }

    private int _score = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        
        // AddUIEventをExtension Methods化して、すぐ使用
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
        
        // TODO. ImageのHandlerを受け取ってドラッグ追加
        GameObject go = GetImage((int)Images.TestImage).gameObject;
        
        BindEvent(go, 
            data => { go.transform.position = data.position; }, 
            Define.UIEvent.Drag);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }
}
