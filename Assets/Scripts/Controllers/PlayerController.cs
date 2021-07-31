using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    PlayerStat _stat;                   //PlayerStat.cs의 PlayerStat 클래스를 변수로 인스턴스화, 2021-07-26

    bool _stopSkill = false;

    public override void Init()
    {
        //Init()을 하자마자, 바로 자기의 Type부터 정해주고 시작, 2021-07-30
        WorldObjectType = Define.WorldObject.Player;

        //지금 현재 이 스크립트 PlayerController.cs를 컴포넌트로 가지고 있는 gameObject에 PlayerStat.cs를 컴포넌트로 추가한 후, _stat 변수에 저장, 2021-07-26
        _stat = gameObject.GetComponent<PlayerStat>();

        //Managers.Resource.Instantiate("UI/UI_Button");

        //Managers.Input.KeyAction += OnKeyboard;을 호출할 때 OnKeyboard() 함수가 두 번 호출 되는것을 막기 위해서 미리 한 번 뺀다, 2021-07-01
        Managers.Input.KeyAction -= OnKeyboard;

        //키보드에 어떠한 키라도 누르면, InputManager한테 OnKeyboard라는 함수를 실행 요청, 2021-07-01
        Managers.Input.KeyAction += OnKeyboard;
        //이렇게 하면 Update()함수는 사용하지 않게 되고, 결국에는 키보드 입력 체크하는 것이 유일하게 된다, 2021-07-01

        //Managers.Input.MouseAction += OnMouseClicked;을 호출할 때 OnMouseClicked() 함수가 두 번 호출 되는것을 막기 위해서 미리 한 번 뺀다, 2021-07-05
        Managers.Input.MouseAction -= OnMouseEvent;

        //마우스에 어떠한 키라도 누르면, InputManager한테 OnMouseClicked라는 함수를 실행 요청, 2021-07-05
        Managers.Input.MouseAction += OnMouseEvent;

        //만약 HPBar가 없으면,
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {
            //HPBar 만들기, 2021-07-28
            //UI_HPBar를 만들어서 transform(= 나)에게 붙이기, 2021-07-28
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }            
    }

    protected override void UpdateSKILL()
    {
        //공격할 때, Monster를 바라보는 방향이 아닌, 다른 방향을 바라보면서 떄릴 때가 있다. 이를 고치는 코드, 2021-07-28
        //마우스를 찍어서 레이저에 맞은 객체가 존재할시, 2021-07-28
        if (_lockTarget != null)
        {
            //거리(방향벡터) = 목적지 위치 - 현재 내 위치, 2021-07-28
            Vector3 dir = _lockTarget.transform.position - transform.position;

            //LockRotation의 매개변수 = 내가 바라보고 싶은 방향, 2021-07-28
            Quaternion quat = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }        
    }

    protected override void UpdateMOVING()
    {
        //마우스를 찍어서 레이저에 맞은 객체가 존재할시, 2021-07-27        
        if (_lockTarget != null)
        {
            //목적지 위치 = 레이저가 도달한 위치
            //레이저가 맞은 객체의 콜라이더의 위치를 목적지 좌표에 저장,
            destinationPosition = _lockTarget.transform.position;

            //거리 = 목적지 위치 - 현재 내 위치, 2021-07-27
            //magnitude : 벡터 크기(거리), normalized : 벡터 방향 
            float distance = (destinationPosition - transform.position).magnitude;

            //목적지 위치와 내 위치가 1 미만이면, 2021-07-27
            if (distance <= 1)
            {
                //공격, 2021-07-27
                State = Define.State.SKILL;
                
                return;
            }
        }

        //방향 벡터 = 목적지 위치 - 현재 내 위치
        Vector3 dir = destinationPosition - transform.position;

        //Player가 Monster 위로 가끔 이동하는 버그를 차단하기 위해 y=0 으로 설정, 2021-07-31
        dir.y = 0;

        //아주 작은값으로 도착했는지 안 했는지 확인, 이 코드를 쓴 이유는 '방향 벡터 = 목적지 위치 - 현재 내 위치'를 해도 정확히 0이 나오지 않는 경우가 많다.(오차가 항상 있다) 2021-07-05
        if (dir.magnitude < 0.1f)
        {           
            State = Define.State.IDLE;
        }

        //else이면, 아직 도착하지 않았다는 뜻, 2021-07-05
        else
        {
            //ray를 시각적으로 나타내기, 2021-07-25
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            
            //만약에 1.0f 앞에 있는 벽에 레이가 닿으면,
            //max distance : 1.0f (코 앞에 있는 벽만 체크), 2021-07-25
            //transform.position + Vector3.up * 0.5f : 뒤에것을 더해주지않으면 발바닥에서 ray를 쏘지만, 더해주면 배꼽 위치에서 쏜다.
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                //Player가 뛰다가 건물에 부딪히면 멈춘다. 디아블로 같은 경우, 부딪혀도 계속 뛰던거 같은데, 디아블로 처럼 만들어보자, 2021-07-26
                //마우스를 계속 누르고 있지 않는 상태라면, 2021-07-26
                if (Input.GetMouseButton(0) == false)
                {
                    //Player의 기본상태를 IDLE로 한다, 2021-07-26
                    State = Define.State.IDLE;
                }

                return;
            }

            //이동하는 값(speed * Time.deltaTime)이, 현재 이동해야 할 남은 거리(dir.magnitude <0.0001f)보다 작아야 한다는 것을 보장 해줘야 함, 2021-07-05
            //Clamp를 쓰면 최소값과 최대값 범위를 지정할 수 있다. 그래서 이동하는 값이 0(최소값) ~ 이동해야 할 남은 거리(최대값) 만큼의 범위로 정하였다, 2021-07-05
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

            //Player가 Monster와 충돌로 인해, Monste가 밀려나고, Monster를 뚫고 지나가지 못해서, NavMeshAgent를 지우고 이걸로 대체, 2021-07-29
            transform.position += dir.normalized * moveDist;

            //Quaternion은 rotation함수라고 봐도 된다. 객체의 rotation에 Slerp(부드럽게 rotation)을 대입, 2021-07-05
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }        
    }

    //ATTACK 애니메이션에 Event추가, 2021-07-27
    //Enemy 체력 깎기, 2021-07-28
    void OnHitEvent()
    {
        Debug.Log("Player OnHitEvent()");

        //Enemy 체력 깎기, 2021-07-28
        //마우스를 찍어서 레이저에 맞은 객체가 존재할시, 2021-07-28
        if (_lockTarget != null)
        {
            //마우스를 찍어서 레이저에 맞은 객체의 Stat.cs 컴포넌트를 Stat을 인스턴스화 한 targetStat 변수에 저장, 2021-07-28
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            //damage를 받아서 hp감소, 2021-07-30
            targetStat.OnAttacked(_stat);
        }
        if (_stopSkill)
        {
            State = Define.State.IDLE;
        }
        else
        {
            State = Define.State.SKILL;
        }
    }
    
    void AnimRun()
    {
        //Animator를 인스턴스화 하여 추가, 2021-07-07
        Animator anim = GetComponent<Animator>();

        State = Define.State.MOVING;
    }

    //, 2021-07-01
    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.5f);

            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;

            AnimRun();

        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.5f);

            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;

            AnimRun();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.5f);

            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;

            AnimRun();
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.5f);

            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;

            AnimRun();
        }

        //keyboard를 눌렀을 때는, player가 목적지로 가는 것과 상관없으니 moveToDestination값을 false로 설정, 2021-07-05 
        //moveToDestination = false;
    }

    //Managers.Input.KeyAction과는 다르게, Managers.Input.MouseAction는 Action에다가 Define.MouseEvent를 넣어준 상태라 Define.MouseEvent evt로 인자를 받아준다, 2021-07-05
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.IDLE:
                OnMouseEvent_IDLERUN(evt);
                break;
            case Define.State.MOVING:
                OnMouseEvent_IDLERUN(evt);
                break;
            case Define.State.SKILL:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                    {
                        _stopSkill = true;
                    }
                }
                break;
        }
    }

    //OnMouseEvent() 함수에 있던 코드들을 여기로 옮겼다.
    //이유 : OnMouseEnvet() 함수에서는 IDLE과 WALK에만 쓸려고 했는데,
    //       스킬을 쓸 때에도 OnMouseEvent가 실행 될 수 있기에
    //       OnMouseEvent_IDLERUN함수를 따로 파줬다, 2021-07-27
    void OnMouseEvent_IDLERUN(Define.MouseEvent evt)
    {
        RaycastHit hit;

        //마우스 위치에 ray를 쏘기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ray를 시각적으로 표시하기
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //Unity창에 바로 접근해서 Unity의 Layer이름을 가져와서 쓰는 것, 2021-07-26
        //LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster");
        //Define.cs의 Layer enum에 적힌 변수들을 가져와서 Layer번호를 이용하여 Layer를 설정하는 것, 2021-07-26
        LayerMask _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

        //ray 발동한 값을 저장
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            //맨 처음 마우스를 뗀 상태에서 마우스를 눌렀을 때, 2021-07-26
            //Unity상에서 기본 제공하는 코드 Input.GetMouseButtonDown(0) 과 같다.
            case Define.MouseEvent.PointerDown:
                {
                    //ray가 발동하여 Ground 혹은 Monster에 맞았다면, 2021-07-26
                    if (raycastHit)
                    {
                        //ray가 닿은 곳(hit.point)를 destinationPosition 변수에 저장, 2021-07-26
                        destinationPosition = hit.point;

                        //캐릭터 상태값은 MOVING, 2021-07-26
                        State = Define.State.MOVING;

                        //Enemy를 클릭 시, 공격을 한 번하고 Player가 멈추는 버그 현상을 고치기 위해 2021-07-27
                        _stopSkill = false;

                        //만약 내가 클릭한 객체가 몬스터라면 ,2021-07-26
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            //마우스로 찍어서 레이저에 맞은 객체의 콜라이더를 _lockTarget변수에 저장, 2021-07-26 
                            //마우스를 찍어서 레이저에 맞은 객체가 존재하니 _lockTarget은 null이 아니다, 2021-07-26
                            _lockTarget = hit.collider.gameObject;
                            Debug.Log("Monster Click!");
                        }

                        //만약 내가 클릭한 객체가 몬스터가 아니라면, 2021-07-26
                        else
                        {
                            //마우스로 찍어서 레이저에 맞은 객체의 콜라이더를 _lockTarget변수에 저장, 2021-07-26 
                            //마우스를 찍어서 레이저에 맞은 객체가 존재하지 않을 시, _lockTarget은 null이다, 2021-07-26
                            _lockTarget = null;
                            Debug.Log("Ground Click!");
                        }
                    }
                }
                break;

            //마우스를 누르고 있는 동안, 2021-07-26
            //Unity상에서 기본 제공하는 코드 Input.GetMouseButton(0) 과 같다.
            case Define.MouseEvent.Press:
                {                    
                    //_lockTarget == null : 마우스를 찍어서 레이저에 맞은 객체가 존재하지 않을시,                    
                    //raycastHit : ray가 발동하여 Ground 혹은 Monster에 맞았다면, 2021-07-26
                    if (_lockTarget == null && raycastHit)
                    {
                        //ray가 닿은 곳(hit.point)를 destinationPosition 변수에 저장, 2021-07-26
                        destinationPosition = hit.point;
                    }
                }
                break;

            //Enemy를 클릭 시, 공격을 한 번하고 Player가 멈추는 버그 현상을 고치기 위해 2021-07-27
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}