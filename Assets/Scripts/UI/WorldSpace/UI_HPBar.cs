using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2021-07-28

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    //ü���� Stat.cs���� �����ϰ� �־���, 2021-07-28
    Stat _stat;

    //UI_Base.cs (�ֻ����θ�)���� Start(){ Init(); }�� �ϴ� ���̹Ƿ�, ���⿡ �ִ� Init()�� ȣ���Ͽ� �������̴�, 2021-07-28
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        //ü���� Stat.cs���� �����ϰ� �־���, 2021-07-28
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        //HPBar�� ��ġ�� �̻��ϴ�, ������ �Ӹ� ���� ��ġ�ϰԲ� ����� ��, 2021-07-28
        //�켱 �θ���� ��ġ�� �̾ƿ;��Ѵ�.
        //gameObject : UI_HPBar.cs�� ����ִ� ���ӿ�����Ʈ�� ����
        //transform : �� ���ӿ�����Ʈ�� Ʈ������(��ġ,��ǥ,ũ��)�� �̾Ƴ� ����,
        //parent : �� ���ӿ�����Ʈ�� �θ� ����
        Transform parent = gameObject.transform.parent;

        //�θ�(ex:Player, Monster)�� ��ġ�� UI_HPBar�� ��ġ(gameObject.transform.position)�� ����
        //�� ������ �ϰԴ�, �θ�(ex:Player, Monster)�� ��ġ���ٰ� y������ ���� ���Ѱ����� �ϴ°� ����.        
        //gameObject.transform.position = parent.position + Vector3.up * 2.0f;        
        //������ : ��ü���� Ű�� �޶� �ϵ��ڵ�� HPBar�� ��ġ�� ��������Ѵ�.
        //�ذ�å : �ݶ��̴��� ��ġ�� �ϸ� ��ü���� Ű�� �ٸ����� �Ӹ� ���� ��Ȯ�� �ø� �� �ִ�.
        gameObject.transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y * 0.5f);

        //Player�� Rotation�� ���ư� ������, HPBar�� Rotation�� ���ư��Ƿ�,
        //�̸� ������Ű�� ����, HPBar�� �׻� ī�޶� �ٶ󺸰� �ϱ� (�ϸ� ������), 2021-07-28 
        //gameObject.transform.LookAt(Camera.main.transform);
        //������� �¿찡 �������� ���װ� �־����Ƿ�, HPBar�� rotation�� ī�޶��� rotation�� �Ȱ��� ���ش�, 2021-07-28
        gameObject.transform.rotation = Camera.main.transform.rotation;

        //ĳ������ ���ִ� ���� : ratio�� float�̱� ������ _stat.Hp / _stat.MaxHP; �� �� int�� ���,
        //�� ���� float���� ����� ������� float�̹Ƿ� ĳ���� �Ѵ�, 2021-07-28 
        float ratio = _stat.Hp / (float)_stat.MaxHP;

        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        //HPBar�� ü���� ���̰� �Ϸ���, �������� HPBar ���ӿ�����Ʈ�� Slider ������Ʈ�� ������ ��,
        //Value�� ���� ��ȭ�� �ָ� �ȴ�, 2021-07-28
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio; 
    }
}