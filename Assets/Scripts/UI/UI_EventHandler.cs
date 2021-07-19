using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;


    //Ŭ���� �ϸ� �̺�Ʈ�� �ѷ��ִ� ���, 2021-07-17
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //���콺�� �̹��� ��� �̵���Ű��, 2021-07-16
        //transform.position = eventData.position;

        //Debug.Log("OnDrag");

        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

    
}
