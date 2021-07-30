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

            //������ üũ
            //Stat.cs�� ������Ƽ Level�� get���� ���� level�� ����, 2021-07-30
            int level = Level;

            while (true)
            {
                //Data.Contents.cs�� namespace Data�� Stat Ŭ������ ���� stat���� �ν��Ͻ�ȭ, 2021-07-30
                Data.Stat stat;

                //Json������ �Ľ� �� ��, Dictionary StatDict�� ���� ����
                //TryGetValue�� ���� ���� ������(��ġ : DataManager.cs�� Dictionary StatDict)
                //false ��� ��, ���� ������ ���ٴ� ��, 2021-07-30
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                {
                    break;
                }

                //���� ����ġ��, ��ü ����ġ���� ������, 2021-07-30
                if(_exp < stat.totalExp)
                {
                    break;
                }

                level++;
            }

            //level�� �ö�, Level�� ���� ���� �ʴٸ�, 2021-07-30
            if (level != Level)
            {
                Debug.Log("Level Up!");
                                
                Level = level;

                //���� ������ ����, ������ �ɷ�ġ�鵵 ������ �°� �ٲ��ش�, 2021-07-30
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

        //������ ������ �� �ڵ���� �Լ��� ���� ���ֱ�, 2021-07-30
        SetStat(_level);
    }

    //���� ������ ����, ������ �ɷ�ġ�鵵 ������ �°� �ٲ��ش�, 2021-07-30
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