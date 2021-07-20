using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//2021-07-20

public abstract class BaseScene : MonoBehaviour
{
    //외부에서 현재 Scene 타입이 무엇인지 궁금할 수도 있기 때문에
    //private은 사용하지 말고
    //get 하는 건 public으로 열어두고,
    //set 하는 건 protected로 보호를 하도록 하자
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        //EventSyetem가 1명이라도 있는지 찾아보기
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EvenySystem";
        }
    }

    public abstract void Clear();
}