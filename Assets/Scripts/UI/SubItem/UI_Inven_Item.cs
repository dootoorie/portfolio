using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base        //독립적으로 뜨는 팝업도 아니고, 하나만 존재하는 개념도 아니니까 UI_Base로 맞춰주자, 2021-07-19
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    string _name;        

    public override void Init()
    {
        //각각 컴포넌트를 찾아서 무는게 아니라, 컴포넌트를 가지고 있는 게임오브젝트 자체를 바인딩, 2021-07-19 
        Bind<GameObject>(typeof(GameObjects));

        //ItemNameText이란 이름의 게임오브젝트 생성 + Text컴포넌트를 가져오면서, 우리가 원하는 텍스트의 내용으로 바꿀 수 있다, 2021-07-19
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;

        //ItemIcon이란 이름의 게임오브젝트 생성 + Image 컴포넌트를 가져와서, 아이템을 클릭 했을 때 {  } 속에 있는 것을 실행, 2021-07-19
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! {_name}"); });
    }

    //Text의 내용을 바꾸는 함수(= 아이템 이름), 2021-07-19
    public void SetInfo(string name)
    {
        _name = name;
    }
}
