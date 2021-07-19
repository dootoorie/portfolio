using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    //����Ƽ�� �����ϴ� ��� object(��ü)�� UnityEngine.Object�� ���� ����(�ֻ��� �θ�) -> �̸� Dictionary�� �����غ���.
    protected Dictionary<Type, UnityEngine.Object[]> dic_objects = new Dictionary<Type, UnityEngine.Object[]>();

    //2021-07-19
    public abstract void Init();

    //Generic <T>
    //enum �� �������� : reflection(���÷���)�� �̿�
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //Enum ������ string���� �̾ƿ��� : enum Texts �ȿ� �ִ� PointText�� ScoreText�� string[]�ȿ� ���� �ȴ�.
        string[] names = Enum.GetNames(type);

        //����Ƽ�� �����ϴ� ��� object(��ü)�� UnityEngine.Object�� ���� ����(�ֻ��� �θ�) -> �̸� Array�� �����غ���.
        UnityEngine.Object[] arr_objects = new UnityEngine.Object[names.Length];

        //Dictionary.Add(key, value);
        dic_objects.Add(typeof(T), arr_objects);

        //���ӿ�����Ʈ�� �巡�� �� ������� ������Ʈ �ȿ� �־��� �۾��� �ڵ�� ���� 
        for (int i = 0; i < names.Length; i++)
        {
            //������Ʈ�� �ƴ� ������Ʈ Ÿ������ �����ϴ� ���, 2021-07-16
            if (typeof(T) == typeof(GameObject))
            {
                //��� FindChild<T> �� �ƴ϶� �׳� FindChild, 2021-07-16
                arr_objects[i] = Util.FindChild(gameObject, names[i], true);
            }

            //������Ʈ Ÿ���� �ƴ� ������Ʈ Ÿ������ �����ϴ� ���
            else
            {
                arr_objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }
            //Utils ������ Util.cs���� ����

            //���� ��ã������ �޽��� ���
            if (arr_objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }

        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] arr_objects = null;

        //�����µ� �����ϸ�
        if (dic_objects.TryGetValue(typeof(T), out arr_objects) == false)
        {
            //���ٴ� �ǹ̷� null
            return null;
        }

        //�����µ� ���������� ������Ʈ�� index�� ������ ����, T�� ĳ����, 2021-07-16
        return arr_objects[idx] as T;
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }


    //2021-07-17
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // UI_EventHandler.cs ������Ʈ�� �߰�
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        
        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }
}