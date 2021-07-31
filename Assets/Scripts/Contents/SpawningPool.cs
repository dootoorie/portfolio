using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//2021-07-31

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;          //���� ����

    int _reserveCount = 0;          //���� �� ���� �����ߴ��� ����� ���� (������ġ)

    [SerializeField]
    int _keepMonsterCount = 0;      //���� ��ü ��

    [SerializeField]
    Vector3 _spawnPos;              //ó�� ������ġ

    [SerializeField]
    float _spawnRadius = 15.0f;     //���� �Ÿ�

    [SerializeField]
    float _spawnTime = 5.0f;        //���� �ð�

    public void AddMonsterCount(int value)
    {
        //���� ����
        _monsterCount += value; 
    }

    public void SetKeepMonsterCount(int count)
    {
        //���� ��ü ��
        _keepMonsterCount = count;
    }

    void Start()
    {
        //������ġ�� - �� �� �ߴٰ� + ���ش�
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        //_reserveCount�� �ʿ��� ���� : _monsterCount�� ���� �������� ���� �����̸�,
        //while���� �������� �� �� �ֱ� ������ ������ ������ �ڵ尡 �ȴ�. �׷��Ƿ�
        //���� �� ���� �����ߴ��� ����� ������ ���Ͽ� �ش�, 2021-07-31 
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    //_spawnTime�� ����ϱ� ���� �ڷ�ƾ, 2021-07-31
    IEnumerator ReserveSpawn()
    {
        _reserveCount++;

        yield return new WaitForSeconds(Random.Range(0, _spawnTime));

        //Monster ����, 2021-07-31
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Zombie");

        //randPos�� �� �� �ִٴ� ������ �����ϱ� ��ã�⸦ �̿��ؼ� �� �� �ִ� �������� �ƴ��� üũ, 2021-07-31
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        //���� ��ġ, 2021-07-31
        Vector3 randPos;

        while (true)
        {
            //Random.insideUnitCircle = 2D : ���� �׷��� ���� ��ǥ�� �ϳ� �̾ƿ´�.
            //Random.insideUnitSphere = 3D : ���� �׷��� ���� ��ǥ�� �ϳ� �̾ƿ´�.
            //_spawnRadius��� �Ÿ��� �������� �����ָ�, �������� ���� ���⺤�Ͱ� ���´�
            //ranDIr = ���ⷣ�� * �Ÿ�����, 2021-07-31
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);

            //���Ͱ� ���� �հ� �Ʒ��� ��������� ���� ����, 2021-07-31
            randDir.y = 0;

            //������ġ = ó�� ������ġ + ���� ���⺤��, 2021-07-31
            randPos = _spawnPos + randDir;

            //�� �� �ֳ�
            NavMeshPath path = new NavMeshPath();

            if (nma.CalculatePath(randPos, path))
            {
                //true�� ���� �� �� �ִ� �����̴ϱ� �ٷ� break, 2021-07-31
                break;
            }
        }

        obj.transform.position = randPos;

        _reserveCount--;
    }
}
