using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//2021-07-20
public class SceneManagerEx     //MonoBehaviour ����
{
    public BaseScene CurrentScene
    {
        get
        {
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    //���� ������ SceneManager.LoadScene�� ��쿡�� string�� �޾Ҿ��µ�,
    //���⼭�� Define.Scene type����, �츮�� ��� �� Scene ��ϵ��� enum ���� �����ϰ� �����ϱ�
    public void LoadScene(Define.Scene type)
    {
        //���� ����ϰ� �ִ� Scene�� ���� ��,
        CurrentScene.Clear();

        //���� ������ �̵�
        SceneManager.LoadScene(GetSceneName(type));     //Login, Game ���.. => SceneManager.LoadScene(Game) ó��
    }

    //LoadScene(Define.Scene type)�Լ�����
    //Define�� Define ������ string�� �ƴϴ�.
    //Scene�� Ÿ���� ������ string�� ����ִ� �Լ��� ������
    //Reflection
    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        
        return name;
    }
}
