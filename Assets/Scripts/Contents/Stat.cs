using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-26

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHP;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHP { get { return _maxHP; } set { _maxHP = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHP = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    //attack �ɷ�ġ�� �ٷ� ���� �ʰ� attacker�� �� ������,
    //player����, monster����, �ٸ� ������ �����ϰ� �� ���� �ֱ� ������, 2021-07-30
    public virtual void OnAttacked(Stat attacker)
    {
        //������ = Player ���� ��ġ - ���콺�� ���� ��ü�� ���潺 ��ġ
        //Enemy�� Defense�� �ʹ� ���Ƽ� Ȥ�ö� ������ �Ǹ� �ȵǴϱ�
        //Mathf.Max(0, ����)�� �̸� ���� Defense�� ������ �Ǵ� ���� ����, 2021-07-30
        int damage = Mathf.Max(0, attacker.Attack - Defense);

        //Hp�� damage������ ��ġ�� �̿��ؼ� ���, 2021-07-30
        Hp -= damage;

        //hp�� 0�� �Ǹ� �ȵǴϱ� ������ ����, 2021-07-30
        if (Hp <= 0)
        {
            Hp = 0;

            OnDead(attacker);
        }               
    }

    //2021-07-30
    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;

        if (playerStat != null)
        {
            playerStat.Exp += 5;
        }              

        Managers.Game.Despawn(gameObject);
    }
}