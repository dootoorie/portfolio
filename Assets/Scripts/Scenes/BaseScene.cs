using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//2021-07-20

public abstract class BaseScene : MonoBehaviour
{
    //�ܺο��� ���� Scene Ÿ���� �������� �ñ��� ���� �ֱ� ������
    //private�� ������� ����
    //get �ϴ� �� public���� ����ΰ�,
    //set �ϴ� �� protected�� ��ȣ�� �ϵ��� ����
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        //EventSyetem�� 1���̶� �ִ��� ã�ƺ���
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EvenySystem";
        }
    }

    public abstract void Clear();
}