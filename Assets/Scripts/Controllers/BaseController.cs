using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-29

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 destinationPosition;        //player�� ������ ��ġ, 2021-07-05       

    [SerializeField]
    protected GameObject _lockTarget;             //���콺�� �� �������� ���� ��ü�� �ݶ��̴��� �����ϴ� ����, 2021-07-26 

    [SerializeField]
    protected Define.State state = Define.State.IDLE;

    //2021-07-30
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    //�������� state�� �ٷ� �����ϴ� ���� �ƴ϶�, ������Ƽ State�� �����Ѵ�
    // - Define.State
    // - anim
    //�� �� ������ �䳢�� �� �������� ������Ƽ ���, 2021-07-27
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

    //Update()���� 1�����Ӵ� ȣ��Ǵ� �Լ��ε�, 60�������� ������ ���ư��� �ִٸ�, 60����1�ʸ��� �������� �̵��ϴ¼�(=ĳ���Ͱ� ������ �̵���), 2021-07-01
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
        //Update()�Լ��� Ű���带 üũ�ϸ�, �� ���ڰ� ��������, ��� Ű���� �Է��� �޾Ҵ��� üũ�ϱ� ����� �κ��� �ִ�.
        //������ KeyAction���� ������, KeyAction.Invoke(); �� �ߴ����� �ɾ, ���� �̺�Ʈ�� �޴���, ���� ������ �Ǵ����� �� �� �ִ�, 2021-07-01            
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