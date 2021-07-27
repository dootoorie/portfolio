using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        IDLE,
        MOVING,
        DIE,
        JUMPING,
        CHANNELING,                     //손을 들고 마법을 쓰고 있는 상태
        FALLING,
        SKILL
    }

    PlayerStat _stat;                   //PlayerStat.cs의 PlayerStat 클래스를 변수로 인스턴스화, 2021-07-26

    Vector3 destinationPosition;        //player의 목적지 위치, 2021-07-05       
    
    GameObject _lockTarget;      //마우스로 찍어서 레이저에 맞은 객체의 콜라이더를 저장하는 변수, 2021-07-26 

    [SerializeField]
    PlayerState state = PlayerState.IDLE;

    //이제부터 state로 바로 접근하는 것이 아니라, 프로퍼티 State로 접근한다
    // - PlayerState
    // - anim
    //이 두 마리의 토끼를 다 잡으려면 프로퍼티 사용, 2021-07-27
    public PlayerState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();

            switch (state)
            {
                case PlayerState.DIE:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.IDLE:
                    anim.SetFloat("speed", 0);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.MOVING:
                    anim.SetFloat("speed", _stat.MoveSpeed);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.SKILL:
                    anim.SetBool("attack", true);
                    break;
            }
        }
    }
    void Start()
    {
        //PlayerStat.cs의 PlayerStat클래스를 변수로 인스턴스화 한 _stat 에다가 PlayerStat.cs 컴포넌트를 붙인다, 2021-07-26
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
    }
       

    void UpdateIDLE()
    {

    }


    void UpdateSKILL()
    {
       
    }

    void UpdateMOVING()
    {
        //마우스를 찍어서 레이저에 맞은 객체가 존재할시, 2021-07-27        
        if (_lockTarget != null)
        {
            //목적지 위치 = 레이저가 도달한 위치
            destinationPosition = _lockTarget.transform.position;

            //거리 = 목적지 위치 - 현재 내 위치, 2021-07-27
            //magnitude : 벡터 크기(거리), normalized : 벡터 방향 
            float distance = (destinationPosition - transform.position).magnitude;

            //목적지 위치와 내 위치가 1 미만이면, 2021-07-27
            if (distance <= 1)
            {
                //공격, 2021-07-27
                State = PlayerState.SKILL;
                
                return;
            }
        }

        //방향 벡터 = 목적지 위치 - 현재 내 위치
        Vector3 dir = destinationPosition - transform.position;

        //아주 작은값으로 도착했는지 안 했는지 확인, 이 코드를 쓴 이유는 '방향 벡터 = 목적지 위치 - 현재 내 위치'를 해도 정확히 0이 나오지 않는 경우가 많다.(오차가 항상 있다) 2021-07-05
        if (dir.magnitude < 0.1f)
        {           
            State = PlayerState.IDLE;
        }

        //else이면, 아직 도착하지 않았다는 뜻, 2021-07-05
        else
        {
            //NavMeshAgent를 변수로 인스턴스화 한 후, NavMeshAgent 컴포넌트를 부착하기, 2021-07-25
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            //이동하는 값(speed * Time.deltaTime)이, 현재 이동해야 할 남은 거리(dir.magnitude <0.0001f)보다 작아야 한다는 것을 보장 해줘야 함, 2021-07-05
            //Clamp를 쓰면 최소값과 최대값 범위를 지정할 수 있다. 그래서 이동하는 값이 0(최소값) ~ 이동해야 할 남은 거리(최대값) 만큼의 범위로 정하였다, 2021-07-05
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

            //NavMeshAgent를 이용해서 이동, 2021-07-25
            nma.Move(dir.normalized * moveDist);

            //player가 이동할 때, 크기가 1로 정규화한 방향벡터에 이동속도를 곱한 값. 2021-07-05
            //transform.position = transform.position + dir.normalized * moveDist;

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
                    State = PlayerState.IDLE;
                }

                return;
            }
            
            //Quaternion은 rotation함수라고 봐도 된다. 객체의 rotation에 Slerp(부드럽게 rotation)을 대입, 2021-07-05
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }        
    }

    void UpdateDIE()
    {
        //아무것도 못함
    }   

    //ATTACK 애니메이션에 Event추가, 2021-07-27
    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        State = PlayerState.IDLE;
    }

    //Update()문은 1프레임당 호출되는 함수인데, 60프레임의 게임이 돌아가고 있다면, 60분의1초마다 포지션을 이동하는셈(=캐릭터가 빠르게 이동됨), 2021-07-01
    void Update()
    {
        switch (state)
        {
            case PlayerState.IDLE:
                UpdateIDLE();
                break;
                
            case PlayerState.MOVING:
                UpdateMOVING();
                break;
                
            case PlayerState.DIE:
                UpdateDIE();
                break;

            case PlayerState.SKILL:
                UpdateSKILL();
                break;
                
        }
        //Update()함수에 키보드를 체크하면, 그 숫자가 많아지면, 어디서 키보드 입력을 받았는지 체크하기 어려운 부분이 있다.
        //하지만 KeyAction으로 받으면, KeyAction.Invoke(); 에 중단점을 걸어서, 누가 이벤트를 받는지, 누가 실행이 되는지도 알 수 있다, 2021-07-01
    }

    void AnimRun()
    {
        //Animator를 인스턴스화 하여 추가, 2021-07-07
        Animator anim = GetComponent<Animator>();
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
        //클릭한 상태 뿐만 아니라, 마우스를 눌리고 있는 상태에서도 이동할 수 있게 이 부분 삭제, 2021-07-07
        //만약 Click이 아니라면, 2021-07-05
        //if (evt != Define.MouseEvent.Click)
        //{
        //    return;
        //}

        //만약 캐릭터 상태가 DIE이면, 반환한다, 2021-07-08
        if (State == PlayerState.DIE)
            return;

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

        switch(evt)
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
                        State = PlayerState.MOVING;

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
                    //마우스를 찍어서 레이저에 맞은 객체가 존재할시,
                    if (_lockTarget != null)
                    {
                        //레이저가 맞은 콜라이더의 위치를 목적지 좌표에 저장, 2021-07-26
                        destinationPosition = _lockTarget.transform.position;
                    }

                    //else if : 마우스를 찍어서 레이저에 맞은 객체가 존재하지 않을 시, 2021-07-26
                    //raycastHit : ray가 발동하여 Ground 혹은 Monster에 맞았다면, 2021-07-26
                    else if (raycastHit)
                    {                        
                        //ray가 닿은 곳(hit.point)를 destinationPosition 변수에 저장, 2021-07-26
                        destinationPosition = hit.point;                        
                    }
                }
                break;
        }        
    }
}