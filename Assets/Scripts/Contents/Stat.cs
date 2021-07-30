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

    //attack 능력치를 바로 받지 않고 attacker로 한 이유는,
    //player인지, monster인지, 다른 정보도 추출하게 될 수도 있기 때문에, 2021-07-30
    public virtual void OnAttacked(Stat attacker)
    {
        //데미지 = Player 어택 수치 - 마우스로 찍은 객체의 디펜스 수치
        //Enemy의 Defense가 너무 높아서 혹시라도 음수가 되면 안되니까
        //Mathf.Max(0, 변수)로 미리 적의 Defense가 음수가 되는 것을 차단, 2021-07-30
        int damage = Mathf.Max(0, attacker.Attack - Defense);

        //Hp를 damage변수의 수치를 이용해서 깎기, 2021-07-30
        Hp -= damage;

        //hp가 0이 되면 안되니까 사전에 차단, 2021-07-30
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
            playerStat.Exp += 15;
        }              

        Managers.Game.Despawn(gameObject);
    }
}