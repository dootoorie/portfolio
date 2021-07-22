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

        //�׽�Ʈ, 2021-07-22
        for (int i = 0; i < 5; i++)
        {
            Managers.Resource.Instantiate("ClazyRunner");
        }

    }

    public override void Clear()
    {

    }
}