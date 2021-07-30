using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Extension 메소드를 쓸 때는, static을 붙여줘야 한다, 2021-07-17
public static class Extension   //MonoBehaviour 제거
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    //2021-07-30
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}