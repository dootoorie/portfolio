using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;               //Action�� ����ϱ� ���� �߰�, 2021-07-01
using UnityEngine.EventSystems;

public class InputManager	//�� ��ũ��Ʈ�� ������Ʈ�� ������ ���� �����̹Ƿ� �Ϲ� C# ��ũ��Ʈ�� �Ѵ�, 2021-07-01
{
    //Action�� ������ delegate, 2021-07-01
    //Ű����� ���õ� Action, 2021-07-01
    public Action KeyAction = null;

    //Define.MouseEvent Ÿ���� �μ��� �޴� �Լ����� ����� �� �ִ� MouseAction �߰�, 2021-07-05
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;       //���콺�� Ŭ���Ǿ����� �ƴ��� �Ǻ��ϴ� ���� ,2021-07-05
    float _pressedTime = 0;      //���콺�� �� �ʵ��� ��������. ���콺�� Click�� PointerUp�� �����ϱ� ����(Define.cs�� enum�� MouseEvent), 2021-07-26
    
    //MonoBehaviour�� �ƴ� ��ũ��Ʈ�̹Ƿ� �������� ���� �ҷ�����ϴ� On�� �ٿ��� �̸��� OnUpdate()�� �ϱ�� �Ѵ�, 2021-07-01
    public void OnUpdate()
    {
        //UI��ư�� Ŭ�� �Ǿ����� �ȵǾ����� �� �� �ִ�. UIŬ�� �� ��Ȳ�̸� return �Ѵ�.
        //�� �ڵ�� ���� UI�� ������ �ص�, ĳ���Ͱ� ���� �������� �ʰ� �ȴ�.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //������ ����(������ ����) : Update() ������ InputManager�� ��ǥ�� �Է��� üũ�� ����, ������ �Է��� �־�����, event�� ���ĸ� ���ִ� ����, 2021-07-01

        //���� � Ű�� �Է��ϰų�, KeyAction �׼ǿ� ��ϵ� �Լ��� �ִٸ� �����Ų�� ,2021-07-01 && 2021-07-05
        if (Input.anyKey && KeyAction != null)
        {
            //��� Ű�� �����ϸ� KeyAction �׼����� ����, 2021-07-01
            KeyAction.Invoke();
            //Update()�Լ��� Ű�� üũ�ϸ�, �� ���ڰ� ��������, ��� Ű �Է��� �޾Ҵ��� üũ�ϱ� ����� �κ��� �ִ�.
            //������ KeyAction���� ������, KeyAction.Invoke(); �� �ߴ����� �ɾ, ���� �̺�Ʈ�� �޴���, ���� ������ �Ǵ����� �� �� �ִ�, 2021-07-01
        }

        //MouseAction�� ��ϵ� �Լ��� �ִٸ� �����Ų�� ,2021-07-05 
        if (MouseAction != null)
        {
            //GetMouseButton�� ���콺�� ������ ���� �� ����, GetMouseButtonDown�� ���콺�� ó�� ������ �� ����, 2021-07-05
            if (Input.GetMouseButton(0))
            {
                //���콺�� �� ���� ������ ���� ���¶��, 2021-07-26
                if(!_pressed)
                {
                    //���콺 ���¸� Click�� �ƴ϶� PointerDown����, 2021-07-26
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);

                    //���콺�� �� �� ���� ���������� Unity���� �ð�����, 2021-07-26
                    _pressedTime = Time.time;
                }
                //Press��� �̺�Ʈ�� �߻�, 2021-07-05
                MouseAction.Invoke(Define.MouseEvent.Press);

                //����, ���콺�� �� ���̶� �������̴�, true, 2021-07-05
                _pressed = true;
            }
            //���콺�� �����ٰ� ���� ��, 2021-07-05
            else
            {
                //���콺�� �� ���̶� ������ pressed�� true�� ���¸�, 2021-07-05
                if (_pressed)
                {
                    //Unity���� �ð��� ���콺�� ������ �־��� �ð� + 0.2�ʺ��� ���� ���, 2021-07-26
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        //���콺 ���¸� PointerDown�� �ƴ϶� Click����, 2021-07-26
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    
                    //Unity���� �ð��� ���콺�� ������ �־��� �ð� + 0.2�ʺ��� ���� ���, ���� ������ �ִٰ� �� ���̴� Click�� �ƴ϶� PointUp, 2021-07-26
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                //true���� pressed�� false�� �ٲ���, 2021-07-05
                _pressed = false;

                //���콺 ���� �ð� �ʱ�ȭ, 2021-07-26
                _pressedTime = 0f;                
            }
        }        
    }

    //�� �̵��� ��, �޸� ������ ����, KeyAction�� MouseAction�� ���õ� ������ �����ֱ�, 2021-07-21
    public void Clear()
    {
        KeyAction = null;

        MouseAction = null;
    }
}