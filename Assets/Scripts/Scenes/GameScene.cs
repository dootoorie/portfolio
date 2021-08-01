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

        //Player 스폰, 2021-07-30
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "ClazyRunner");

        //Player 스폰시, 카메라 세팅, 2021-07-30
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        //Transform player2 = transform.Find("ClazyRunner");

        //FreeLookController free = new FreeLookController();

        //free.gameObject.GetOrAddComponent<FreeLookController>().SetPlayer(player2);

        //Monster 스폰, 2021-07-30
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Zombie");

        //Monster 스폰, 2021-07-31
        GameObject go = new GameObject { name = "SpawningPool" };

        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();

        //몬스터 갯수가 5마리가 생성 될 때까지 계속 스포닝풀에서 스포닝을 한다, 2021-07-31
        pool.SetKeepMonsterCount(5);
                
    }

    public override void Clear()
    {

    }
}