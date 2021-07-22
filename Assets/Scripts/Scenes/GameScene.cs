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

        //Test.cs에서 쓰던 코드를 이제 당당하게 여기에서 쓰자
        Managers.UI.ShowSceneUI<UI_Inven>();

        //테스트, 2021-07-22
        for (int i = 0; i < 5; i++)
        {
            Managers.Resource.Instantiate("ClazyRunner");
        }

    }

    public override void Clear()
    {

    }
}