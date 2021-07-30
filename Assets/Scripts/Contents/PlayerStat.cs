using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-26, 2021-07-30

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp 
    { 
        get 
        { 
            return _exp; 
        } 
    
        set 
        { 
            _exp = value;

            //레벨업 체크
            //Stat.cs의 프로퍼티 Level의 get값을 변수 level에 저장, 2021-07-30
            int level = Level;

            while (true)
            {
                //Data.Contents.cs의 namespace Data의 Stat 클래스를 변수 stat으로 인스턴스화, 2021-07-30
                Data.Stat stat;

                //Json파일을 파싱 한 후, Dictionary StatDict에 넣은 것을
                //TryGetValue를 통해 값을 꺼내기(위치 : DataManager.cs의 Dictionary StatDict)
                //false 라는 건, 다음 레벨이 없다는 뜻, 2021-07-30
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                {
                    break;
                }

                //현재 경험치가, 전체 경험치보다 적으면, 2021-07-30
                if(_exp < stat.totalExp)
                {
                    break;
                }

                level++;
            }

            //level이 올라, Level과 값이 같지 않다면, 2021-07-30
            if (level != Level)
            {
                Debug.Log("Level Up!");
                                
                Level = level;

                //오른 레벨에 따라, 나머지 능력치들도 레벨에 맞게 바꿔준다, 2021-07-30
                SetStat(Level);
            }
        } 
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        _moveSpeed = 5.0f;
        _gold = 0;        

        //스탯을 변수로 한 코드들은 함수로 따로 빼주기, 2021-07-30
        SetStat(_level);
    }

    //오른 레벨에 따라, 나머지 능력치들도 레벨에 맞게 바꿔준다, 2021-07-30
    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        Data.Stat stat = dict[1];

        _hp = stat.maxHp;
        _maxHP = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;
    }

    //2021-07-30
    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}