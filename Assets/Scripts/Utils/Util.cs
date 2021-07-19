using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util   //MonoBehaviour ����
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

    //������Ʈ�� �ƴ� ������Ʈ�� �޾Ƽ� �����ϴ� ���, 2021-07-16
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        //��� GameObject�� Transform�� ������ �ִ�.
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
    }

    //������Ʈ�� �ƴ� ������Ʈ�� �޾Ƽ� �����ϴ� ���
    //recursive = false : ��������� ã�� ���ΰ�
    //(���� ������Ʈ ã�� ��, ���� �ڽĸ� ã�������� vs �����ڽ��� �ڽı��� ã�� ������ ���� ��)
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
