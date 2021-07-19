using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;                //player �̵��ӵ� , 2021-07-01        
    //bool moveToDestination = false;     //player�� Ŭ���� ������ �̵����� ���� ����, 2021-07-05
    Vector3 destinationPosition;        //player�� ���� ��ġ, 2021-07-05
    //float idle_run_ratio = 0;           //Blend Tree ����(IDLE_RUN), 2021-07-07


    public enum PlayerState
    {
        IDLE,
        MOVING,
        DIE,
        JUMPING,
        CHANNELING,     //���� ��� ������ ���� �ִ� ����
        FALLING,
    }

    PlayerState state = PlayerState.IDLE;

    void UpdateIDLE()
    {
        //float������ idle_run_ratio�� Lerp�� ���� �Ų����� �ִϸ��̼��� �ٲٱ�, 2021-07-07 
        //idle_run_ratio = Mathf.Lerp(idle_run_ratio, 0, 10.0f * Time.deltaTime);
                
        //Blend Tree�� float ���� idle_run_ratio�� 0�� ��, �� �ֱ� ��� ����, 2021-07-07
        //anim.SetFloat("idle_run_ratio", 0);

        //Blend Tree �̸� IDLE_RUN�� �̿��ؼ� �� �ֱ�, 2021-07-07
        //anim.Play("IDLE_RUN");

        //Animator�� �ν��Ͻ�ȭ �Ͽ� �߰�, 2021-07-08
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void UpdateMOVING()
    {
        //���� ���� = ������ ��ġ - ���� �� ��ġ
        Vector3 dir = destinationPosition - transform.position;

        //���� ���������� �����ߴ��� �� �ߴ��� Ȯ��, �� �ڵ带 �� ������ '���� ���� = ������ ��ġ - ���� �� ��ġ'�� �ص� ��Ȯ�� 0�� ������ �ʴ� ��찡 ����.(������ �׻� �ִ�) 2021-07-05
        if (dir.magnitude < 0.0001f)
        {           
            state = PlayerState.IDLE;
        }

        //else�̸�, ���� �������� �ʾҴٴ� ��, 2021-07-05
        else
        {
            //�̵��ϴ� ��(speed * Time.deltaTime)��, ���� �̵��ؾ� �� ���� �Ÿ�(dir.magnitude <0.0001f)���� �۾ƾ� �Ѵٴ� ���� ���� ����� ��, 2021-07-05
            //Clamp�� ���� �ּҰ��� �ִ밪 ������ ������ �� �ִ�. �׷��� �̵��ϴ� ���� 0(�ּҰ�) ~ �̵��ؾ� �� ���� �Ÿ�(�ִ밪) ��ŭ�� ������ ���Ͽ���, 2021-07-05
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);

            //player�� �̵��� ��, ũ�Ⱑ 1�� ����ȭ�� ���⺤�Ϳ� �̵��ӵ��� ���� ��. 2021-07-05
            transform.position = transform.position + dir.normalized * moveDist;

            //Quaternion�� rotation�Լ���� ���� �ȴ�. ��ü�� rotation�� Slerp(�ε巴�� rotation)�� ����, 2021-07-05
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", speed);
    }

    void UpdateDIE()
    {
        //�ƹ��͵� ����
    }

    void Start()
    {
        //Managers.Resource.Instantiate("UI/UI_Button");

        //Managers.Input.KeyAction += OnKeyboard;�� ȣ���� �� OnKeyboard() �Լ��� �� �� ȣ�� �Ǵ°��� ���� ���ؼ� �̸� �� �� ����, 2021-07-01
        Managers.Input.KeyAction -= OnKeyboard;

        //Ű���忡 ��� Ű�� ������, InputManager���� OnKeyboard��� �Լ��� ���� ��û, 2021-07-01
        Managers.Input.KeyAction += OnKeyboard;
        //�̷��� �ϸ� Update()�Լ��� ������� �ʰ� �ǰ�, �ᱹ���� Ű���� �Է� üũ�ϴ� ���� �����ϰ� �ȴ�, 2021-07-01

        //Managers.Input.MouseAction += OnMouseClicked;�� ȣ���� �� OnMouseClicked() �Լ��� �� �� ȣ�� �Ǵ°��� ���� ���ؼ� �̸� �� �� ����, 2021-07-05
        Managers.Input.MouseAction -= OnMouseClicked;

        //���콺�� ��� Ű�� ������, InputManager���� OnMouseClicked��� �Լ��� ���� ��û, 2021-07-05
        Managers.Input.MouseAction += OnMouseClicked;
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
                
        }
        //Update()�Լ��� Ű���带 üũ�ϸ�, �� ���ڰ� ��������, ��� Ű���� �Է��� �޾Ҵ��� üũ�ϱ� ����� �κ��� �ִ�.
        //������ KeyAction���� ������, KeyAction.Invoke(); �� �ߴ����� �ɾ, ���� �̺�Ʈ�� �޴���, ���� ������ �Ǵ����� �� �� �ִ�, 2021-07-01
    }

    void AnimRun()
    {
        //�ִϸ��̼�

        //float������ idle_run_ratio�� Lerp�� ���� �Ų����� �ִϸ��̼��� �ٲٱ�, 2021-07-07 
        //idle_run_ratio = Mathf.Lerp(idle_run_ratio, 1, 10.0f * Time.deltaTime);
        //Animator�� �ν��Ͻ�ȭ �Ͽ� �߰�, 2021-07-07
        Animator anim = GetComponent<Animator>();

        //Blend Tree�� float ���� idle_run_ratio�� 1�� ��, �޸��� ��� ����, 2021-07-07
        //anim.SetFloat("idle_run_ratio", 1);

        //Blend Tree �̸� IDLE_RUN�� �̿��ؼ� �ٱ�, 2021-07-07
        //anim.Play("IDLE_RUN");
    }

    //, 2021-07-01
    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.5f);

            transform.position += Vector3.forward * Time.deltaTime * speed;

            AnimRun();

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.5f);

            transform.position += Vector3.back * Time.deltaTime * speed;

            AnimRun();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.5f);

            transform.position += Vector3.left * Time.deltaTime * speed;

            AnimRun();
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.5f);

            transform.position += Vector3.right * Time.deltaTime * speed;

            AnimRun();
        }

        //keyboard�� ������ ����, player�� �������� ���� �Ͱ� ��������� moveToDestination���� false�� ����, 2021-07-05 
        //moveToDestination = false;
    }

    //Managers.Input.KeyAction���� �ٸ���, Managers.Input.MouseAction�� Action���ٰ� Define.MouseEvent�� �־��� ���¶� Define.MouseEvent evt�� ���ڸ� �޾��ش�, 2021-07-05
    void OnMouseClicked(Define.MouseEvent evt)
    {
        //Ŭ���� ���� �Ӹ� �ƴ϶�, ���콺�� ������ �ִ� ���¿����� �̵��� �� �ְ� �� �κ� ����, 2021-07-07
        //���� Click�� �ƴ϶��, 2021-07-05
        //if (evt != Define.MouseEvent.Click)
        //{
        //    return;
        //}

        //���� ĳ���� ���°� DIE�̸�, ��ȯ�Ѵ�, 2021-07-08
        if (state == PlayerState.DIE)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
                
        LayerMask mask = LayerMask.GetMask("Ground");

        RaycastHit hit;

        //ray�� ���� mask������ �ִ� ���� ������, 2021-07-05
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            //ray�� ���� ��(hit.point)�� destinationPosition ������ �ִ´�, 2021-07-05
            destinationPosition = hit.point;
            //moveToDestination false���� true�� �ٲ��ش�, 2021-07-05

            //ĳ���� ���°��� MOVING, 2021-07-08
            state = PlayerState.MOVING;
        }
    }
}