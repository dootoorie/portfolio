using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-23

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager  //MonoBehaviour ����
{
    //���ͳ� NPC�� ��� �̸� �ĺ��ϴ� ID�� �ʿ��ѵ�, List�� ��������� ȿ���� ��������
    //List ���� ���, Ư�� ID�� ã�� ���� ��, ��� ���� �ϳ��ϳ� �� ��ȸ�� �غ��� �� �ۿ� ����.
    //��κ��� ���, DataManager�� Dictionary�� ������ ���� ��찡 ȿ�����̴�.
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    

    public void Init()
    {
        //StatData : KeyŸ�԰� ValueŸ�� ��ü�� ����ִ� Ŭ����
        //int : Key Ÿ��
        //Stat : Value Ÿ��
        StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        //Unity���� �����ϴ� Json�Ľ�
        //FromJson : Json�� Class�� ��ȯ
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}