using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-29

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 destinationPosition;        //player의 목적지 위치, 2021-07-05       

    [SerializeField]
    protected GameObject _lockTarget;             //마우스로 찍어서 레이저에 맞은 객체의 콜라이더를 저장하는 변수, 2021-07-26 

    [SerializeField]
    protected Define.State state = Define.State.IDLE;

    //2021-07-30
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    //이제부터 state로 바로 접근하는 것이 아니라, 프로퍼티 State로 접근한다
    // - Define.State
    // - anim
    //이 두 마리의 토끼를 다 잡으려면 프로퍼티 사용, 2021-07-27
    public virtual Define.State State
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
                case Define.State.DIE:
                    break;
                case Define.State.IDLE:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case Define.State.MOVING:
                    anim.CrossFade("WALK", 0.1f);
                    break;
                case Define.State.SKILL:
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }        

    //Update()문은 1프레임당 호출되는 함수인데, 60프레임의 게임이 돌아가고 있다면, 60분의1초마다 포지션을 이동하는셈(=캐릭터가 빠르게 이동됨), 2021-07-01
    void Update()
    {
        switch (State)
        {
            case Define.State.DIE:
                UpdateDIE();
                break;

            case Define.State.MOVING:
                UpdateMOVING();
                break;

            case Define.State.IDLE:
                UpdateIDLE();
                break;

            case Define.State.SKILL:
                UpdateSKILL();
                break;

        }
        //Update()함수에 키보드를 체크하면, 그 숫자가 많아지면, 어디서 키보드 입력을 받았는지 체크하기 어려운 부분이 있다.
        //하지만 KeyAction으로 받으면, KeyAction.Invoke(); 에 중단점을 걸어서, 누가 이벤트를 받는지, 누가 실행이 되는지도 알 수 있다, 2021-07-01            
    }

    public abstract void Init();

    protected virtual void UpdateDIE()
    {

    }

    protected virtual void UpdateMOVING()
    {
        
    }

    protected virtual void UpdateIDLE()
    {

    }

    protected virtual void UpdateSKILL()
    {

    }
}