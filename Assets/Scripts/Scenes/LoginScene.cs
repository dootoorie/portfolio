using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//2021-07-20
public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        //LoginScene -> GameScene 으로 이동하는 것을 구현 해본 적 있으니 여기다가 테스트, 2021-07-22
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            list.Add(Managers.Resource.Instantiate("ClazyRunner"));
        }

        foreach(GameObject obj in list)
        {
            Managers.Resource.Destroy(obj);
        }    
    }

    private void Update()
    {
        //로그인 Scene에서 게임 Scene으로 넘어가기
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //비동기식 - 백그라운드에서 데이터를 로드 할 수 있어서 로딩화면 만들 때 유용
            //SceneManager.LoadSceneAsync("Game");

            //동기식 - 로그인 Scene을 날리고 그 후, 게임 Scene을 하나하나 로딩을 시작(규모가 큰 MMORPG는 한참을 기다려야 함)
            //SceneManager.LoadScene("Game");

            //새로 만든 SceneManagerEx.cs와 Define의 enum값을 이용해서 Scene 전환(Login 씬 -> Game 씬)
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

}