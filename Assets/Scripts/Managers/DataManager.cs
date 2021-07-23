using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-23

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager  //MonoBehaviour 삭제
{
    //몬스터나 NPC의 경우 이를 식별하는 ID가 필요한데, List로 들고있으면 효율이 떨어진다
    //List 같은 경우, 특정 ID를 찾고 싶을 때, 모든 값을 하나하나 다 순회를 해보는 수 밖에 없다.
    //대부분의 경우, DataManager는 Dictionary로 가지고 있을 경우가 효율적이다.
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    

    public void Init()
    {
        //StatData : Key타입과 Value타입 전체를 들고있는 클래스
        //int : Key 타입
        //Stat : Value 타입
        StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        //Unity에서 제공하는 Json파싱
        //FromJson : Json을 Class로 변환
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}