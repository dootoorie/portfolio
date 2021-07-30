using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;          //주변을 스캔하는 범위, 2021-07-29
    
    [SerializeField]
    float _attackRange = 2;         //공격 범위, 2021-07-29

    public override void Init()
    {
        //Init()을 하자마자, 바로 자기의 Type부터 정해주고 시작, 2021-07-30
        WorldObjectType = Define.WorldObject.Monster;

        //지금 현재 이 스크립트 MonsterController.cs를 컴포넌트로 가지고 있는 gameObject에 Stat.cs를 컴포넌트로 추가한 후, _stat 변수에 저장, 2021-07-29
        _stat = gameObject.GetComponent<Stat>();

        //HPBar 만들기, 2021-07-29
        //만약 HPBar가 없으면,
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {            
            //UI_HPBar를 만들어서 transform(= Monster)에게 붙이기, 2021-07-29
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
    }

    protected override void UpdateIDLE()
    {
        Debug.Log("Monster UpdateIDLE");

        //태그가 Player인 게임 오브젝트를 player변수에 저장, 2021-07-29
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject player = Managers.Game.GetPlayer();

        if (player == null)
        {
            return;
        }

        //거리 = 태그가 Player인 게임 오브젝트의 위치 - Monster 위치
        float distance = (player.transform.position - transform.position).magnitude;

        //거리가 주변을 스캔하는 범위보다 가까우면, 2021-07-29
        if(distance <= _scanRange)
        {
            //player를 타겟 변수에 저장, 2021-07-29
            _lockTarget = player;

            //상태변화는 MOVING으로 변화, 2021-07-29
            State = Define.State.MOVING;

            return;
        }
    }

    protected override void UpdateMOVING()
    {
        //타겟인 Player가 존재한다면, 2021-07-29
        if (_lockTarget != null)
        {            
            //목적지 좌표 = Player의 좌표, 2021-07-29
            destinationPosition = _lockTarget.transform.position;

            //거리 = 목적지 위치 - 현재 내 위치, 2021-07-29
            //magnitude : 벡터 크기(거리), normalized : 벡터 방향 
            float distance = (destinationPosition - transform.position).magnitude;

            //목적지 위치와 내 위치가 1 미만이면, 2021-07-29
            if (distance <= _attackRange)
            {
                //NavMeshAgent를 변수로 인스턴스화 한 후, NavMeshAgent 컴포넌트를 부착하기, 2021-07-29
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

                //NavMeshAgent를 이용하여 목적지로 이동, 2021-07-29
                nma.SetDestination(transform.position);

                //공격, 2021-07-29
                State = Define.State.SKILL;

                return;
            }
        }

        //방향 벡터 = 목적지 위치 - 현재 내 위치
        Vector3 dir = destinationPosition - transform.position;

        //아주 작은값으로 도착했는지 안 했는지 확인, 이 코드를 쓴 이유는 '방향 벡터 = 목적지 위치 - 현재 내 위치'를 해도 정확히 0이 나오지 않는 경우가 많다.(오차가 항상 있다), 2021-07-29
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.IDLE;
        }

        //else이면, 아직 도착하지 않았다는 뜻, 2021-07-29
        else
        {
            //NavMeshAgent를 변수로 인스턴스화 한 후, NavMeshAgent 컴포넌트를 부착하기, 2021-07-29
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            //NavMeshAgent를 이용하여 목적지로 이동, 2021-07-29
            nma.SetDestination(destinationPosition);

            //NavMeshAgent를 이용하여 speed 설정, 2021-07-29
            nma.speed = _stat.MoveSpeed;

            //Quaternion은 rotation함수라고 봐도 된다. 객체의 rotation에 Slerp(부드럽게 rotation)을 대입, 2021-07-29
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateSKILL()
    {
        //공격할 때, Monster를 바라보는 방향이 아닌, 다른 방향을 바라보면서 떄릴 때가 있다. 이를 고치는 코드, 2021-07-29
        //마우스를 찍어서 레이저에 맞은 객체가 존재할시, 2021-07-29
        if (_lockTarget != null)
        {
            //거리(방향벡터) = 목적지 위치 - 현재 내 위치, 2021-07-29
            Vector3 dir = _lockTarget.transform.position - transform.position;

            //LockRotation의 매개변수 = 내가 바라보고 싶은 방향, 2021-07-29
            Quaternion quat = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    //애니메이션에 OnHitEvent() 함수를 넣을 거기 때문에, 선언과 정의를 해놓아야 에러가 발생하지 않는다, 2021-07-29
    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent()");

        if (_lockTarget != null)
        {
            //타겟(=Player)이 Stat.cs 스크립트 컴포넌트를 추가하고, Stat.cs을 인스턴스화 한 targetStat변수에 저장, 2021-07-29
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            //damage를 받아서 hp감소, 2021-07-30
            targetStat.OnAttacked(_stat);
                       
            if (targetStat.Hp > 0)
            {
                //방향벡터에 magnitude를 하면 크기가 나온다, 2021-07-29
                float distance = (_lockTarget.transform.position - transform.position).magnitude;

                if (distance <= _attackRange)
                {
                    State = Define.State.SKILL;
                }

                else
                {
                    State = Define.State.MOVING;
                }
            }

            else
            {
                State = Define.State.IDLE;
            }
        }

        else
        {
            State = Define.State.IDLE;
        }
    }
}