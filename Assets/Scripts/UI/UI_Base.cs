using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    //유니티에 존재하는 모든 object(객체)는 UnityEngine.Object에 저장 가능(최상위 부모) -> 이를 Dictionary로 관리해보자.
    protected Dictionary<Type, UnityEngine.Object[]> dic_objects = new Dictionary<Type, UnityEngine.Object[]>();

    //2021-07-19
    public abstract void Init();

    //Generic <T>
    //enum 값 가져오기 : reflection(리플렉션)을 이용
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //Enum 값들을 string으로 뽑아오기 : enum Texts 안에 있는 PointText와 ScoreText가 string[]안에 들어가게 된다.
        string[] names = Enum.GetNames(type);

        //유니티에 존재하는 모든 object(객체)는 UnityEngine.Object에 저장 가능(최상위 부모) -> 이를 Array로 관리해보자.
        UnityEngine.Object[] arr_objects = new UnityEngine.Object[names.Length];

        //Dictionary.Add(key, value);
        dic_objects.Add(typeof(T), arr_objects);

        //게임오브젝트를 드래그 앤 드롭으로 컴포넌트 안에 넣었던 작업을 코드로 구현 
        for (int i = 0; i < names.Length; i++)
        {
            //컴포넌트가 아닌 오브젝트 타입으로 연동하는 경우, 2021-07-16
            if (typeof(T) == typeof(GameObject))
            {
                //얘는 FindChild<T> 가 아니라 그냥 FindChild, 2021-07-16
                arr_objects[i] = Util.FindChild(gameObject, names[i], true);
            }

            //오브젝트 타입이 아닌 컴포넌트 타입으로 연동하는 경우
            else
            {
                arr_objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }
            //Utils 폴더의 Util.cs에서 구현

            //만약 못찾았으면 메시지 출력
            if (arr_objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }

        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] arr_objects = null;

        //꺼내는데 실패하면
        if (dic_objects.TryGetValue(typeof(T), out arr_objects) == false)
        {
            //없다는 의미로 null
            return null;
        }

        //꺼내는데 성공했으면 오브젝트의 index를 추출한 다음, T로 캐스팅, 2021-07-16
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
        // UI_EventHandler.cs 컴포넌트를 추가
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
