using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Extension �޼ҵ带 �� ����, static�� �ٿ���� �Ѵ�, 2021-07-17
public static class Extension   //MonoBehaviour ����
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.AddUIEvent(go, action, type);
    }
}