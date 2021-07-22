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

        //LoginScene -> GameScene ���� �̵��ϴ� ���� ���� �غ� �� ������ ����ٰ� �׽�Ʈ, 2021-07-22
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
        //�α��� Scene���� ���� Scene���� �Ѿ��
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //�񵿱�� - ��׶��忡�� �����͸� �ε� �� �� �־ �ε�ȭ�� ���� �� ����
            //SceneManager.LoadSceneAsync("Game");

            //����� - �α��� Scene�� ������ �� ��, ���� Scene�� �ϳ��ϳ� �ε��� ����(�Ը� ū MMORPG�� ������ ��ٷ��� ��)
            //SceneManager.LoadScene("Game");

            //���� ���� SceneManagerEx.cs�� Define�� enum���� �̿��ؼ� Scene ��ȯ(Login �� -> Game ��)
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

}