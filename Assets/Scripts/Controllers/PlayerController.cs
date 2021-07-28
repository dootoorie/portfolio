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
        CHANNELING,                     //���� ��� ������ ���� �ִ� ����
        FALLING,
        SKILL
    }

    PlayerStat _stat;                   //PlayerStat.cs�� PlayerStat Ŭ������ ������ �ν��Ͻ�ȭ, 2021-07-26

    Vector3 destinationPosition;        //player�� ������ ��ġ, 2021-07-05       
    
    GameObject _lockTarget;      //���콺�� �� �������� ���� ��ü�� �ݶ��̴��� �����ϴ� ����, 2021-07-26 

    [SerializeField]
    PlayerState state = PlayerState.IDLE;

    //�������� state�� �ٷ� �����ϴ� ���� �ƴ϶�, ������Ƽ State�� �����Ѵ�
    // - PlayerState
    // - anim
    //�� �� ������ �䳢�� �� �������� ������Ƽ ���, 2021-07-27
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
                    break;
                case PlayerState.IDLE:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case PlayerState.MOVING:
                    anim.CrossFade("WALK", 0.1f);
                    break;
                case PlayerState.SKILL:
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }
    void Start()
    {
        //PlayerStat.cs�� PlayerStatŬ������ ������ �ν��Ͻ�ȭ �� _stat ���ٰ� PlayerStat.cs ������Ʈ�� ���δ�, 2021-07-26
        _stat = gameObject.GetComponent<PlayerStat>();

        //Managers.Resource.Instantiate("UI/UI_Button");

        //Managers.Input.KeyAction += OnKeyboard;�� ȣ���� �� OnKeyboard() �Լ��� �� �� ȣ�� �Ǵ°��� ���� ���ؼ� �̸� �� �� ����, 2021-07-01
        Managers.Input.KeyAction -= OnKeyboard;

        //Ű���忡 ��� Ű�� ������, InputManager���� OnKeyboard��� �Լ��� ���� ��û, 2021-07-01
        Managers.Input.KeyAction += OnKeyboard;
        //�̷��� �ϸ� Update()�Լ��� ������� �ʰ� �ǰ�, �ᱹ���� Ű���� �Է� üũ�ϴ� ���� �����ϰ� �ȴ�, 2021-07-01

        //Managers.Input.MouseAction += OnMouseClicked;�� ȣ���� �� OnMouseClicked() �Լ��� �� �� ȣ�� �Ǵ°��� ���� ���ؼ� �̸� �� �� ����, 2021-07-05
        Managers.Input.MouseAction -= OnMouseEvent;

        //���콺�� ��� Ű�� ������, InputManager���� OnMouseClicked��� �Լ��� ���� ��û, 2021-07-05
        Managers.Input.MouseAction += OnMouseEvent;

        //HPBar �����, 2021-07-28
        //UI_HPBar�� ���� transform(= ��)���� ���̱�, 2021-07-28
        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
       

    void UpdateIDLE()
    {

    }


    void UpdateSKILL()
    {
        //������ ��, Monster�� �ٶ󺸴� ������ �ƴ�, �ٸ� ������ �ٶ󺸸鼭 ���� ���� �ִ�. �̸� ��ġ�� �ڵ�, 2021-07-28
        //���콺�� �� �������� ���� ��ü�� �����ҽ�, 2021-07-28
        if (_lockTarget != null)
        {
            //�Ÿ�(���⺤��) = ������ ��ġ - ���� �� ��ġ, 2021-07-28
            Vector3 dir = _lockTarget.transform.position - transform.position;

            //LockRotation�� �Ű����� = ���� �ٶ󺸰� ���� ����, 2021-07-28
            Quaternion quat = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }        
    }

    void UpdateMOVING()
    {
        //���콺�� �� �������� ���� ��ü�� �����ҽ�, 2021-07-27        
        if (_lockTarget != null)
        {
            //������ ��ġ = �������� ������ ��ġ
            //�������� ���� ��ü�� �ݶ��̴��� ��ġ�� ������ ��ǥ�� ����,
            destinationPosition = _lockTarget.transform.position;

            //�Ÿ� = ������ ��ġ - ���� �� ��ġ, 2021-07-27
            //magnitude : ���� ũ��(�Ÿ�), normalized : ���� ���� 
            float distance = (destinationPosition - transform.position).magnitude;

            //������ ��ġ�� �� ��ġ�� 1 �̸��̸�, 2021-07-27
            if (distance <= 1)
            {
                //����, 2021-07-27
                State = PlayerState.SKILL;
                
                return;
            }
        }

        //���� ���� = ������ ��ġ - ���� �� ��ġ
        Vector3 dir = destinationPosition - transform.position;

        //���� ���������� �����ߴ��� �� �ߴ��� Ȯ��, �� �ڵ带 �� ������ '���� ���� = ������ ��ġ - ���� �� ��ġ'�� �ص� ��Ȯ�� 0�� ������ �ʴ� ��찡 ����.(������ �׻� �ִ�) 2021-07-05
        if (dir.magnitude < 0.1f)
        {           
            State = PlayerState.IDLE;
        }

        //else�̸�, ���� �������� �ʾҴٴ� ��, 2021-07-05
        else
        {
            //NavMeshAgent�� ������ �ν��Ͻ�ȭ �� ��, NavMeshAgent ������Ʈ�� �����ϱ�, 2021-07-25
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            //�̵��ϴ� ��(speed * Time.deltaTime)��, ���� �̵��ؾ� �� ���� �Ÿ�(dir.magnitude <0.0001f)���� �۾ƾ� �Ѵٴ� ���� ���� ����� ��, 2021-07-05
            //Clamp�� ���� �ּҰ��� �ִ밪 ������ ������ �� �ִ�. �׷��� �̵��ϴ� ���� 0(�ּҰ�) ~ �̵��ؾ� �� ���� �Ÿ�(�ִ밪) ��ŭ�� ������ ���Ͽ���, 2021-07-05
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

            //NavMeshAgent�� �̿��ؼ� �̵�, 2021-07-25
            nma.Move(dir.normalized * moveDist);

            //player�� �̵��� ��, ũ�Ⱑ 1�� ����ȭ�� ���⺤�Ϳ� �̵��ӵ��� ���� ��. 2021-07-05
            //transform.position = transform.position + dir.normalized * moveDist;

            //ray�� �ð������� ��Ÿ����, 2021-07-25
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            
            //���࿡ 1.0f �տ� �ִ� ���� ���̰� ������,
            //max distance : 1.0f (�� �տ� �ִ� ���� üũ), 2021-07-25
            //transform.position + Vector3.up * 0.5f : �ڿ����� �������������� �߹ٴڿ��� ray�� ������, �����ָ� ��� ��ġ���� ���.
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                //Player�� �ٴٰ� �ǹ��� �ε����� �����. ��ƺ�� ���� ���, �ε����� ��� �ٴ��� ������, ��ƺ�� ó�� ������, 2021-07-26
                //���콺�� ��� ������ ���� �ʴ� ���¶��, 2021-07-26
                if (Input.GetMouseButton(0) == false)
                {
                    //Player�� �⺻���¸� IDLE�� �Ѵ�, 2021-07-26
                    State = PlayerState.IDLE;
                }

                return;
            }
            
            //Quaternion�� rotation�Լ���� ���� �ȴ�. ��ü�� rotation�� Slerp(�ε巴�� rotation)�� ����, 2021-07-05
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }        
    }

    void UpdateDIE()
    {
        //�ƹ��͵� ����
    }   

    //ATTACK �ִϸ��̼ǿ� Event�߰�, 2021-07-27
    //Enemy ü�� ���, 2021-07-28
    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        //Enemy ü�� ���, 2021-07-28
        //���콺�� �� �������� ���� ��ü�� �����ҽ�, 2021-07-28
        if (_lockTarget != null)
        {
            //���콺�� �� �������� ���� ��ü�� Stat.cs ������Ʈ�� Stat�� �ν��Ͻ�ȭ �� targetStat ������ ����, 2021-07-28
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            //PlayerController.cs �� ��ũ��Ʈ�� ���� gameObject(=Player)�� ���� PlayerStat.cs ������Ʈ�� PlayerStat�� �ν��Ͻ�ȭ�� myStat ������ ����, 2021-07-28
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();

            //������ = Player ���� ��ġ - ���콺�� ���� ��ü�� ���潺 ��ġ
            //Enemy�� Defense�� �ʹ� ���Ƽ� Ȥ�ö� ������ �Ǹ� �ȵǴϱ�
            //Mathf.Max(0, ����)�� �̸� ���� Defense�� ������ �Ǵ� ���� ����, 2021-07-28 
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);

            Debug.Log(damage);

            //���콺�� �� �������� ���� ��ü�� Hp�� damage������ ��ġ�� �̿��ؼ� ���, 2021-07-28
            targetStat.Hp -= damage;
        }
        if (_stopSkill)
        {
            State = PlayerState.IDLE;
        }
        else
        {
            State = PlayerState.SKILL;
        }
    }

    //Update()���� 1�����Ӵ� ȣ��Ǵ� �Լ��ε�, 60�������� ������ ���ư��� �ִٸ�, 60����1�ʸ��� �������� �̵��ϴ¼�(=ĳ���Ͱ� ������ �̵���), 2021-07-01
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
        //Update()�Լ��� Ű���带 üũ�ϸ�, �� ���ڰ� ��������, ��� Ű���� �Է��� �޾Ҵ��� üũ�ϱ� ����� �κ��� �ִ�.
        //������ KeyAction���� ������, KeyAction.Invoke(); �� �ߴ����� �ɾ, ���� �̺�Ʈ�� �޴���, ���� ������ �Ǵ����� �� �� �ִ�, 2021-07-01
    }

    void AnimRun()
    {
        //Animator�� �ν��Ͻ�ȭ �Ͽ� �߰�, 2021-07-07
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

        //keyboard�� ������ ����, player�� �������� ���� �Ͱ� ��������� moveToDestination���� false�� ����, 2021-07-05 
        //moveToDestination = false;
    }

    bool _stopSkill = false;

    //Managers.Input.KeyAction���� �ٸ���, Managers.Input.MouseAction�� Action���ٰ� Define.MouseEvent�� �־��� ���¶� Define.MouseEvent evt�� ���ڸ� �޾��ش�, 2021-07-05
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case PlayerState.IDLE:
                OnMouseEvent_IDLERUN(evt);
                break;
            case PlayerState.MOVING:
                OnMouseEvent_IDLERUN(evt);
                break;
            case PlayerState.SKILL:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                    {
                        _stopSkill = true;
                    }
                }
                break;
        }
    }

    //OnMouseEvent() �Լ��� �ִ� �ڵ���� ����� �Ű��.
    //���� : OnMouseEnvet() �Լ������� IDLE�� WALK���� ������ �ߴµ�,
    //       ��ų�� �� ������ OnMouseEvent�� ���� �� �� �ֱ⿡
    //       OnMouseEvent_IDLERUN�Լ��� ���� �����, 2021-07-27
    void OnMouseEvent_IDLERUN(Define.MouseEvent evt)
    {
        RaycastHit hit;

        //���콺 ��ġ�� ray�� ���
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ray�� �ð������� ǥ���ϱ�
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //Unityâ�� �ٷ� �����ؼ� Unity�� Layer�̸��� �����ͼ� ���� ��, 2021-07-26
        //LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster");
        //Define.cs�� Layer enum�� ���� �������� �����ͼ� Layer��ȣ�� �̿��Ͽ� Layer�� �����ϴ� ��, 2021-07-26
        LayerMask _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

        //ray �ߵ��� ���� ����
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            //�� ó�� ���콺�� �� ���¿��� ���콺�� ������ ��, 2021-07-26
            //Unity�󿡼� �⺻ �����ϴ� �ڵ� Input.GetMouseButtonDown(0) �� ����.
            case Define.MouseEvent.PointerDown:
                {
                    //ray�� �ߵ��Ͽ� Ground Ȥ�� Monster�� �¾Ҵٸ�, 2021-07-26
                    if (raycastHit)
                    {
                        //ray�� ���� ��(hit.point)�� destinationPosition ������ ����, 2021-07-26
                        destinationPosition = hit.point;

                        //ĳ���� ���°��� MOVING, 2021-07-26
                        State = PlayerState.MOVING;

                        //Enemy�� Ŭ�� ��, ������ �� ���ϰ� Player�� ���ߴ� ���� ������ ��ġ�� ���� 2021-07-27
                        _stopSkill = false;

                        //���� ���� Ŭ���� ��ü�� ���Ͷ�� ,2021-07-26
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            //���콺�� �� �������� ���� ��ü�� �ݶ��̴��� _lockTarget������ ����, 2021-07-26 
                            //���콺�� �� �������� ���� ��ü�� �����ϴ� _lockTarget�� null�� �ƴϴ�, 2021-07-26
                            _lockTarget = hit.collider.gameObject;
                            Debug.Log("Monster Click!");
                        }

                        //���� ���� Ŭ���� ��ü�� ���Ͱ� �ƴ϶��, 2021-07-26
                        else
                        {
                            //���콺�� �� �������� ���� ��ü�� �ݶ��̴��� _lockTarget������ ����, 2021-07-26 
                            //���콺�� �� �������� ���� ��ü�� �������� ���� ��, _lockTarget�� null�̴�, 2021-07-26
                            _lockTarget = null;
                            Debug.Log("Ground Click!");
                        }
                    }
                }
                break;

            //���콺�� ������ �ִ� ����, 2021-07-26
            //Unity�󿡼� �⺻ �����ϴ� �ڵ� Input.GetMouseButton(0) �� ����.
            case Define.MouseEvent.Press:
                {                    
                    //_lockTarget == null : ���콺�� �� �������� ���� ��ü�� �������� ������,                    
                    //raycastHit : ray�� �ߵ��Ͽ� Ground Ȥ�� Monster�� �¾Ҵٸ�, 2021-07-26
                    if (_lockTarget == null && raycastHit)
                    {
                        //ray�� ���� ��(hit.point)�� destinationPosition ������ ����, 2021-07-26
                        destinationPosition = hit.point;
                    }
                }
                break;

            //Enemy�� Ŭ�� ��, ������ �� ���ϰ� Player�� ���ߴ� ���� ������ ��ġ�� ���� 2021-07-27
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}