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
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    

    public void Init()
    {
        //Json������ �Ľ� �� ��, Dictionary StatDict�� �ֱ�
        //Data.StatData : KeyŸ�԰� ValueŸ�� ��ü�� ����ִ� StatData Ŭ����(��ġ : Data.Contents.cs ��ũ��Ʈ�� ���ӽ����̽� Data�� StatData Ŭ����)
        //int : Key Ÿ��
        //Data.Stat : Value Ÿ�� (��ġ : Data ���ӽ����̽��� Stat Ŭ����)
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        //Unity���� �����ϴ� Json�Ľ�
        //FromJson : Json�� Class�� ��ȯ
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}