using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//2021-07-31

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;          //몬스터 갯수

    int _reserveCount = 0;          //몬스터 몇 개를 예약했는지 예약된 갯수 (안전장치)

    [SerializeField]
    int _keepMonsterCount = 0;      //몬스터 개체 수

    [SerializeField]
    Vector3 _spawnPos;              //처음 스폰위치

    [SerializeField]
    float _spawnRadius = 15.0f;     //스폰 거리

    [SerializeField]
    float _spawnTime = 5.0f;        //스폰 시간

    public void AddMonsterCount(int value)
    {
        //몬스터 갯수
        _monsterCount += value; 
    }

    public void SetKeepMonsterCount(int count)
    {
        //몬스터 개체 수
        _keepMonsterCount = count;
    }

    void Start()
    {
        //안전장치로 - 한 번 했다가 + 해준다
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        //_reserveCount가 필요한 이유 : _monsterCount가 아직 생성되지 않은 상태이면,
        //while문이 무한으로 돌 수 있기 때문에 굉장히 위험한 코드가 된다. 그러므로
        //몬스터 몇 개를 예약했는지 예약된 갯수를 더하여 준다, 2021-07-31 
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    //_spawnTime을 사용하기 위해 코루틴, 2021-07-31
    IEnumerator ReserveSpawn()
    {
        _reserveCount++;

        yield return new WaitForSeconds(Random.Range(0, _spawnTime));

        //Monster 스폰, 2021-07-31
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Zombie");

        //randPos에 갈 수 있다는 보장이 없으니까 길찾기를 이용해서 갈 수 있는 영역인지 아닌지 체크, 2021-07-31
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        //랜덤 위치, 2021-07-31
        Vector3 randPos;

        while (true)
        {
            //Random.insideUnitCircle = 2D : 원을 그려서 랜덤 좌표를 하나 뽑아온다.
            //Random.insideUnitSphere = 3D : 원을 그려서 랜덤 좌표를 하나 뽑아온다.
            //_spawnRadius라는 거리를 마지막에 곱해주면, 랜덤으로 뽑은 방향벡터가 나온다
            //ranDIr = 방향랜덤 * 거리랜덤, 2021-07-31
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);

            //몬스터가 땅을 뚫고 아래에 만들어지는 것을 방지, 2021-07-31
            randDir.y = 0;

            //랜덤위치 = 처음 스폰위치 + 랜덤 방향벡터, 2021-07-31
            randPos = _spawnPos + randDir;

            //갈 수 있나
            NavMeshPath path = new NavMeshPath();

            if (nma.CalculatePath(randPos, path))
            {
                //true를 뱉어내면 갈 수 있는 영역이니까 바로 break, 2021-07-31
                break;
            }
        }

        obj.transform.position = randPos;

        _reserveCount--;
    }
}
