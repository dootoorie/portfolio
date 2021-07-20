using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//2021-07-20
public class SceneManagerEx     //MonoBehaviour 삭제
{
    public BaseScene CurrentScene
    {
        get
        {
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    //원래 원본인 SceneManager.LoadScene의 경우에는 string을 받았었는데,
    //여기서는 Define.Scene type으로, 우리가 사용 할 Scene 목록들을 enum 으로 관리하고 있으니까
    public void LoadScene(Define.Scene type)
    {
        //현재 사용하고 있는 Scene을 날린 후,
        CurrentScene.Clear();

        //다음 씬으로 이동
        SceneManager.LoadScene(GetSceneName(type));     //Login, Game 등등.. => SceneManager.LoadScene(Game) 처럼
    }

    //LoadScene(Define.Scene type)함수에서
    //Define은 Define 값이지 string이 아니다.
    //Scene의 타입을 넣으면 string을 뱉어주는 함수를 만들자
    //Reflection
    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        
        return name;
    }
}
