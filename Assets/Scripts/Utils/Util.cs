using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util   //MonoBehaviour 삭제
{
    //2021-07-17
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    //컴포넌트가 아닌 오브젝트를 받아서 연동하는 경우, 2021-07-16
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        //모든 GameObject는 Transform을 가지고 있다.
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
    }

    //오브젝트가 아닌 컴포넌트를 받아서 연동하는 경우
    //recursive = false : 재귀적으로 찾을 것인가
    //(게임 오브젝트 찾을 때, 하위 자식만 찾을것인지 vs 하위자식의 자식까지 찾을 것인지 묻는 것)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {


        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {


                Transform transform = go.transform.GetChild(0);



                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();

                    if(component !=null)
                    {
                        return component;
                    }
                }
            }
            
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
                    
            }
        }

        return null;
    }
}
