using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;                //player 이동속도 , 2021-07-01        
    //bool moveToDestination = false;     //player를 클릭한 곳으로 이동할지 말지 여부, 2021-07-05
    Vector3 destinationPosition;        //player의 도착 위치, 2021-07-05
    //float idle_run_ratio = 0;           //Blend Tree 변수(IDLE_RUN), 2021-07-07


    public enum PlayerState
    {
        IDLE,
        MOVING,
        DIE,
        JUMPING,
        CHANNELING,     //손을 들고 마법을 쓰고 있는 상태
        FALLING,
    }

    PlayerState state = PlayerState.IDLE;

    void UpdateIDLE()
    {
        //float변수인 idle_run_ratio에 Lerp를 통해 매끄럽게 애니메이션을 바꾸기, 2021-07-07 
        //idle_run_ratio = Mathf.Lerp(idle_run_ratio, 0, 10.0f * Time.deltaTime);
                
        //Blend Tree의 float 변수 idle_run_ratio가 0일 시, 서 있기 모션 적용, 2021-07-07
        //anim.SetFloat("idle_run_ratio", 0);

        //Blend Tree 이름 IDLE_RUN을 이용해서 서 있기, 2021-07-07
        //anim.Play("IDLE_RUN");

        //Animator를 인스턴스화 하여 추가, 2021-07-08
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void UpdateMOVING()
    {
        //방향 벡터 = 목적지 위치 - 현재 내 위치
        Vector3 dir = destinationPosition - transform.position;

        //아주 작은값으로 도착했는지 안 했는지 확인, 이 코드를 쓴 이유는 '방향 벡터 = 목적지 위치 - 현재 내 위치'를 해도 정확히 0이 나오지 않는 경우가 많다.(오차가 항상 있다) 2021-07-05
        if (dir.magnitude < 0.1f)
        {           
            state = PlayerState.IDLE;
        }

        //else이면, 아직 도착하지 않았다는 뜻, 2021-07-05
        else
        {

            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            //이동하는 값(speed * Time.deltaTime)이, 현재 이동해야 할 남은 거리(dir.magnitude <0.0001f)보다 작아야 한다는 것을 보장 해줘야 함, 2021-07-05
            //Clamp를 쓰면 최소값과 최대값 범위를 지정할 수 있다. 그래서 이동하는 값이 0(최소값) ~ 이동해야 할 남은 거리(최대값) 만큼의 범위로 정하였다, 2021-07-05
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);

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
                //상태는 IDLE로 바뀐다.
                state = PlayerState.IDLE;

                return;
            }

            
                //Quaternion은 rotation함수라고 봐도 된다. 객체의 rotation에 Slerp(부드럽게 rotation)을 대입, 2021-07-05
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", speed);
    }

    void UpdateDIE()
    {
        //아무것도 못함
    }

    void Start()
    {
        //Managers.Resource.Instantiate("UI/UI_Button");

        //Managers.Input.KeyAction += OnKeyboard;을 호출할 때 OnKeyboard() 함수가 두 번 호출 되는것을 막기 위해서 미리 한 번 뺀다, 2021-07-01
        Managers.Input.KeyAction -= OnKeyboard;

        //키보드에 어떠한 키라도 누르면, InputManager한테 OnKeyboard라는 함수를 실행 요청, 2021-07-01
        Managers.Input.KeyAction += OnKeyboard;
        //이렇게 하면 Update()함수는 사용하지 않게 되고, 결국에는 키보드 입력 체크하는 것이 유일하게 된다, 2021-07-01

        //Managers.Input.MouseAction += OnMouseClicked;을 호출할 때 OnMouseClicked() 함수가 두 번 호출 되는것을 막기 위해서 미리 한 번 뺀다, 2021-07-05
        Managers.Input.MouseAction -= OnMouseClicked;

        //마우스에 어떠한 키라도 누르면, InputManager한테 OnMouseClicked라는 함수를 실행 요청, 2021-07-05
        Managers.Input.MouseAction += OnMouseClicked;
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
                
        }
        //Update()함수에 키보드를 체크하면, 그 숫자가 많아지면, 어디서 키보드 입력을 받았는지 체크하기 어려운 부분이 있다.
        //하지만 KeyAction으로 받으면, KeyAction.Invoke(); 에 중단점을 걸어서, 누가 이벤트를 받는지, 누가 실행이 되는지도 알 수 있다, 2021-07-01
    }

    void AnimRun()
    {
        //애니메이션

        //float변수인 idle_run_ratio에 Lerp를 통해 매끄럽게 애니메이션을 바꾸기, 2021-07-07 
        //idle_run_ratio = Mathf.Lerp(idle_run_ratio, 1, 10.0f * Time.deltaTime);
        //Animator를 인스턴스화 하여 추가, 2021-07-07
        Animator anim = GetComponent<Animator>();

        //Blend Tree의 float 변수 idle_run_ratio가 1일 시, 달리기 모션 적용, 2021-07-07
        //anim.SetFloat("idle_run_ratio", 1);

        //Blend Tree 이름 IDLE_RUN을 이용해서 뛰기, 2021-07-07
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

        //keyboard를 눌렀을 때는, player가 목적지로 가는 것과 상관없으니 moveToDestination값을 false로 설정, 2021-07-05 
        //moveToDestination = false;
    }

    //Managers.Input.KeyAction과는 다르게, Managers.Input.MouseAction는 Action에다가 Define.MouseEvent를 넣어준 상태라 Define.MouseEvent evt로 인자를 받아준다, 2021-07-05
    void OnMouseClicked(Define.MouseEvent evt)
    {
        //클릭한 상태 뿐만 아니라, 마우스를 눌리고 있는 상태에서도 이동할 수 있게 이 부분 삭제, 2021-07-07
        //만약 Click이 아니라면, 2021-07-05
        //if (evt != Define.MouseEvent.Click)
        //{
        //    return;
        //}

        //만약 캐릭터 상태가 DIE이면, 반환한다, 2021-07-08
        if (state == PlayerState.DIE)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
                
        LayerMask mask = LayerMask.GetMask("Ground");

        RaycastHit hit;

        //ray를 쏴서 mask변수에 있는 값에 닿으면, 2021-07-05
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            //ray가 닿은 곳(hit.point)를 destinationPosition 변수에 넣는다, 2021-07-05
            destinationPosition = hit.point;
            //moveToDestination false값을 true로 바꿔준다, 2021-07-05

            //캐릭터 상태값은 MOVING, 2021-07-08
            state = PlayerState.MOVING;
        }
    }
}