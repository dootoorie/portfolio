using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-20
public class GameScene : BaseScene
{    
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Test.cs���� ���� �ڵ带 ���� ����ϰ� ���⿡�� ����
        Managers.UI.ShowSceneUI<UI_Inven>();

        //���� ������, 2021-07-23
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        //GameScene.cs ������Ʈ�� ������ �ִ� ���ӿ�����Ʈ���� CursorController.cs�� ���δ�, 2021-07-27
        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }
}