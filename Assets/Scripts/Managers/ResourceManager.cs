using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager //이 스크립트는 컴포넌트로 만들지 않을 예정이므로 일반 C# 스크립트로 한다.
{
    //where T : Object : 부모 클래스가 Object인 타입만 받을 수 있도록 제약을 걸음
   public T Load<T>(string path) where T : Object
    {
        //프리팹인 경우, original을 찾아보고 바로 반환하자


        //만약 T가 GameObject와 일치하면 프리팹일 확률이 굉장히 높을테니 찾아보자
        if(typeof(T) == typeof(GameObject))
        {
            //프리팹이 저장되어 있는 경로를 name으로 저장
            string name = path;

            //경로 같은 경우, abc/Knight 등 슬래쉬(/)로 되어 있는데, 맨 마지막 슬래쉬(/)를 찾아서 그 뒷부분만 추출하면 된다.
            //맨 마지막 슬래쉬(/) 찾는 방법 : LastIndexOf()를 이용
            int index = name.LastIndexOf('/');

            //만약 index가 존재하면,
            if (index >= 0)
            {
                //Substring(int startIndex) : 문자열 자르기.
                //Substring을 이용하여 name 저장.
                //index + 1을 한 이유는, index까지가 슬래쉬(/)이므로, 슬래쉬(/) 뒷 부분부터 잘라서 저장하기 위해
                name = name.Substring(index + 1);
            }

            //운좋게 GetOriginal을 찾았으면
            GameObject go = Managers.Pool.GetOriginal(name);

            //반환
            if (go != null)
            {
                return go as T;
            }
        }

        //Resources 폴더를 시작 위치로 한 "path"에 해당하는 T 타입의 에셋 파일을 불러오고 이를 리턴한다.
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 1. original 이미 들고 있으면 바로 사용
        //Load를 사용해 prefab에 path에 해당하는 GameObejct 타입의 에셋을 할당한다
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        //만약 원본 프리팹이 없으면
        if (original == null)
        {
            //오류 메시지 나타내기({path} : 경로포함)
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //원래 Instantiate할 때 앞에 안 붙였는데,
        //Object를 붙인 이유는 Instantiate만 있으면 12번째 줄을 재귀적으로 호출하려고 하니까
        //Object에 있는 Instantiate를 하라는 정확한 코드
        //return Object.Instantiate(prefab, parent);

        // 2. 혹시 Pooling 된 애가 있을까?

        //만약 Poolable 컴포넌트를 가지고 있는 원본 프리팹이 있으면 , (Poolable 컴포넌트가 없으면 Pooling 하는 대상이 아니니까)
        if (original.GetComponent<Poolable>() != null)
        {
            // Pop : 대기하고 있는 Pooling 된 오브젝트가 있는지 확인하여, 있으면 바로 사용하겠다는 뜻 
            return Managers.Pool.Pop(original, parent).gameObject;
        }
        //하이어라키에 프리팹 생성. 그리고 게임오브젝트 go에 저장, 2021-07-19
        GameObject go = Object.Instantiate(original, parent);

        //원본을 복사한 상태, 201-07-21
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // 만약에 Pooling이 필요한 아이라면 -> PoolManager 한테 위탁

        //일단 Pooling 대상인지 확인하자(= Poolable 컴포넌트를 가지고 있는지 확인)
        Poolable poolable = go.GetComponent<Poolable>();

        //Destroy 하기 전에 점검
        //만약 Pooling대상이면,(= Poolable 컴포넌트를 가지고 있다면)
        if (poolable != null)
        {
            // Push : Pooling 된 오브젝트를 다 사용한 다음에, 반환하는 작업
            Managers.Pool.Push(poolable);

            return;
        }

        //만약 Pooling 대상이 아니라면, Destroy
        Object.Destroy(go);
    }

}