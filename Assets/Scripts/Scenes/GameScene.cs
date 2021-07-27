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

        //스탯 데이터, 2021-07-23
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        //GameScene.cs 컴포넌트를 가지고 있는 게임오브젝트에게 CursorController.cs를 붙인다, 2021-07-27
        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }
}