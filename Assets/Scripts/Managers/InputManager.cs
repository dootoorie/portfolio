using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;               //Action을 사용하기 위해 추가, 2021-07-01
using UnityEngine.EventSystems;

public class InputManager	//이 스크립트는 컴포넌트로 만들지 않을 예정이므로 일반 C# 스크립트로 한다, 2021-07-01
{
    //Action은 일종의 delegate, 2021-07-01
    //키보드와 관련된 Action, 2021-07-01
    public Action KeyAction = null;

    //Define.MouseEvent 타입의 인수만 받는 함수들을 등록할 수 있는 MouseAction 추가, 2021-07-05
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;       //마우스가 클릭되었는지 아닌지 판별하는 변수 ,2021-07-05
    float _pressedTime = 0;      //마우스를 몇 초동안 눌렀는지. 마우스의 Click과 PointerUp을 구분하기 위해(Define.cs의 enum인 MouseEvent), 2021-07-26
    
    //MonoBehaviour가 아닌 스크립트이므로 누군가가 직접 불러줘야하니 On을 붙여서 이름을 OnUpdate()로 하기로 한다, 2021-07-01
    public void OnUpdate()
    {
        //UI버튼이 클릭 되었는지 안되었는지 알 수 있다. UI클릭 된 상황이면 return 한다.
        //이 코드로 인해 UI를 누른다 해도, 캐릭터가 같이 움직이지 않게 된다.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //리스너 패턴(디자인 패턴) : Update() 문에서 InputManager가 대표로 입력을 체크한 다음, 실제로 입력이 있었으면, event로 전파를 해주는 형식, 2021-07-01

        //만약 어떤 키를 입력하거나, KeyAction 액션에 등록된 함수가 있다면 실행시킨다 ,2021-07-01 && 2021-07-05
        if (Input.anyKey && KeyAction != null)
        {
            //어떠한 키라도 반응하면 KeyAction 액션으로 전파, 2021-07-01
            KeyAction.Invoke();
            //Update()함수에 키를 체크하면, 그 숫자가 많아지면, 어디서 키 입력을 받았는지 체크하기 어려운 부분이 있다.
            //하지만 KeyAction으로 받으면, KeyAction.Invoke(); 에 중단점을 걸어서, 누가 이벤트를 받는지, 누가 실행이 되는지도 알 수 있다, 2021-07-01
        }

        //MouseAction에 등록된 함수가 있다면 실행시킨다 ,2021-07-05 
        if (MouseAction != null)
        {
            //GetMouseButton은 마우스를 누르고 있을 때 반응, GetMouseButtonDown은 마우스를 처음 눌렀을 때 반응, 2021-07-05
            if (Input.GetMouseButton(0))
            {
                //마우스를 한 번도 누르지 않은 상태라면, 2021-07-26
                if(!_pressed)
                {
                    //마우스 상태를 Click이 아니라 PointerDown으로, 2021-07-26
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);

                    //마우스를 몇 초 동안 눌렀는지는 Unity상의 시간으로, 2021-07-26
                    _pressedTime = Time.time;
                }
                //Press라는 이벤트를 발생, 2021-07-05
                MouseAction.Invoke(Define.MouseEvent.Press);

                //이제, 마우스를 한 번이라도 누른것이니, true, 2021-07-05
                _pressed = true;
            }
            //마우스를 눌렀다가 뗐을 때, 2021-07-05
            else
            {
                //마우스를 한 번이라도 눌러서 pressed가 true인 상태면, 2021-07-05
                if (_pressed)
                {
                    //Unity상의 시간이 마우스를 눌리고 있었던 시간 + 0.2초보다 낮을 경우, 2021-07-26
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        //마우스 상태를 PointerDown이 아니라 Click으로, 2021-07-26
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    
                    //Unity상의 시간이 마우스를 눌리고 있었던 시간 + 0.2초보다 높을 경우, 한참 누르고 있다가 뗀 것이니 Click이 아니라 PointUp, 2021-07-26
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                //true였던 pressed는 false로 바꿔줌, 2021-07-05
                _pressed = false;

                //마우스 누른 시간 초기화, 2021-07-26
                _pressedTime = 0f;                
            }
        }        
    }

    //씬 이동할 때, 메모리 관리를 위해, KeyAction과 MouseAction과 관련된 데이터 날려주기, 2021-07-21
    public void Clear()
    {
        KeyAction = null;

        MouseAction = null;
    }
}