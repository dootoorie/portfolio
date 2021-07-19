using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager //이 스크립트는 컴포넌트로 만들지 않을 예정이므로 일반 C# 스크립트로 한다.
{
    //where T : Object : 부모 클래스가 Object인 타입만 받을 수 있도록 제약을 걸음
   public T Load<T>(string path) where T : Object
    {
        //Resources 폴더를 시작 위치로 한 "path"에 해당하는 T 타입의 에셋 파일을 불러오고 이를 리턴한다.
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        //Load를 사용해 prefab에 path에 해당하는 GameObejct 타입의 에셋을 할당한다
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        //프리팹이 없으면
        if (prefab == null)
        {
            //오류 메시지 나타내기({path} : 경로포함)
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //원래 Instantiate할 때 앞에 안 붙였는데,
        //Object를 붙인 이유는 Instantiate만 있으면 12번째 줄을 재귀적으로 호출하려고 하니까
        //Object에 있는 Instantiate를 하라는 정확한 코드
        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }

}
