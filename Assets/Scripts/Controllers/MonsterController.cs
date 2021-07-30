using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;          //�ֺ��� ��ĵ�ϴ� ����, 2021-07-29
    
    [SerializeField]
    float _attackRange = 2;         //���� ����, 2021-07-29

    public override void Init()
    {
        //Init()�� ���ڸ���, �ٷ� �ڱ��� Type���� �����ְ� ����, 2021-07-30
        WorldObjectType = Define.WorldObject.Monster;

        //���� ���� �� ��ũ��Ʈ MonsterController.cs�� ������Ʈ�� ������ �ִ� gameObject�� Stat.cs�� ������Ʈ�� �߰��� ��, _stat ������ ����, 2021-07-29
        _stat = gameObject.GetComponent<Stat>();

        //HPBar �����, 2021-07-29
        //���� HPBar�� ������,
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {            
            //UI_HPBar�� ���� transform(= Monster)���� ���̱�, 2021-07-29
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
    }

    protected override void UpdateIDLE()
    {
        Debug.Log("Monster UpdateIDLE");

        //�±װ� Player�� ���� ������Ʈ�� player������ ����, 2021-07-29
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject player = Managers.Game.GetPlayer();

        if (player == null)
        {
            return;
        }

        //�Ÿ� = �±װ� Player�� ���� ������Ʈ�� ��ġ - Monster ��ġ
        float distance = (player.transform.position - transform.position).magnitude;

        //�Ÿ��� �ֺ��� ��ĵ�ϴ� �������� ������, 2021-07-29
        if(distance <= _scanRange)
        {
            //player�� Ÿ�� ������ ����, 2021-07-29
            _lockTarget = player;

            //���º�ȭ�� MOVING���� ��ȭ, 2021-07-29
            State = Define.State.MOVING;

            return;
        }
    }

    protected override void UpdateMOVING()
    {
        //Ÿ���� Player�� �����Ѵٸ�, 2021-07-29
        if (_lockTarget != null)
        {            
            //������ ��ǥ = Player�� ��ǥ, 2021-07-29
            destinationPosition = _lockTarget.transform.position;

            //�Ÿ� = ������ ��ġ - ���� �� ��ġ, 2021-07-29
            //magnitude : ���� ũ��(�Ÿ�), normalized : ���� ���� 
            float distance = (destinationPosition - transform.position).magnitude;

            //������ ��ġ�� �� ��ġ�� 1 �̸��̸�, 2021-07-29
            if (distance <= _attackRange)
            {
                //NavMeshAgent�� ������ �ν��Ͻ�ȭ �� ��, NavMeshAgent ������Ʈ�� �����ϱ�, 2021-07-29
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

                //NavMeshAgent�� �̿��Ͽ� �������� �̵�, 2021-07-29
                nma.SetDestination(transform.position);

                //����, 2021-07-29
                State = Define.State.SKILL;

                return;
            }
        }

        //���� ���� = ������ ��ġ - ���� �� ��ġ
        Vector3 dir = destinationPosition - transform.position;

        //���� ���������� �����ߴ��� �� �ߴ��� Ȯ��, �� �ڵ带 �� ������ '���� ���� = ������ ��ġ - ���� �� ��ġ'�� �ص� ��Ȯ�� 0�� ������ �ʴ� ��찡 ����.(������ �׻� �ִ�), 2021-07-29
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.IDLE;
        }

        //else�̸�, ���� �������� �ʾҴٴ� ��, 2021-07-29
        else
        {
            //NavMeshAgent�� ������ �ν��Ͻ�ȭ �� ��, NavMeshAgent ������Ʈ�� �����ϱ�, 2021-07-29
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            //NavMeshAgent�� �̿��Ͽ� �������� �̵�, 2021-07-29
            nma.SetDestination(destinationPosition);

            //NavMeshAgent�� �̿��Ͽ� speed ����, 2021-07-29
            nma.speed = _stat.MoveSpeed;

            //Quaternion�� rotation�Լ���� ���� �ȴ�. ��ü�� rotation�� Slerp(�ε巴�� rotation)�� ����, 2021-07-29
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateSKILL()
    {
        //������ ��, Monster�� �ٶ󺸴� ������ �ƴ�, �ٸ� ������ �ٶ󺸸鼭 ���� ���� �ִ�. �̸� ��ġ�� �ڵ�, 2021-07-29
        //���콺�� �� �������� ���� ��ü�� �����ҽ�, 2021-07-29
        if (_lockTarget != null)
        {
            //�Ÿ�(���⺤��) = ������ ��ġ - ���� �� ��ġ, 2021-07-29
            Vector3 dir = _lockTarget.transform.position - transform.position;

            //LockRotation�� �Ű����� = ���� �ٶ󺸰� ���� ����, 2021-07-29
            Quaternion quat = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    //�ִϸ��̼ǿ� OnHitEvent() �Լ��� ���� �ű� ������, ����� ���Ǹ� �س��ƾ� ������ �߻����� �ʴ´�, 2021-07-29
    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent()");

        if (_lockTarget != null)
        {
            //Ÿ��(=Player)�� Stat.cs ��ũ��Ʈ ������Ʈ�� �߰��ϰ�, Stat.cs�� �ν��Ͻ�ȭ �� targetStat������ ����, 2021-07-29
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            //damage�� �޾Ƽ� hp����, 2021-07-30
            targetStat.OnAttacked(_stat);
                       
            if (targetStat.Hp > 0)
            {
                //���⺤�Ϳ� magnitude�� �ϸ� ũ�Ⱑ ���´�, 2021-07-29
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