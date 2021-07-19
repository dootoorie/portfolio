using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;


    //클릭을 하면 이벤트를 뿌려주는 방식, 2021-07-17
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //마우스로 이미지 끌어서 이동시키기, 2021-07-16
        //transform.position = eventData.position;

        //Debug.Log("OnDrag");

        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

    
}
