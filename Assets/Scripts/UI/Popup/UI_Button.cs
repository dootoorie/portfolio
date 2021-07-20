using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//2021-07-08
public class UI_Button : UI_Popup
{
    //< UI자동화 >
    //리플렉션 : C#에는 리플렉션이라는 어마어마한 기능이 있음(=일종의 엑스레이 기능)

    //enum을 어떻게 다른곳에 넘겨줄 수 있을까? -> reflection(리플렉션)
    enum Buttons
    {
        PointButton
    }

    //enum을 어떻게 다른곳에 넘겨줄 수 있을까? -> reflection(리플렉션)
    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    
    private void Start()
    {
        Init();
    }


    public override void Init()
    {
        //부모의 Init()을 가져온다.
        base.Init();

        //Generic <T> : Button컴포넌트를 물고있는 게임오브젝트를 찾을 것이다.
        Bind<Button>(typeof(Buttons));

        //Generic <T> : Text컴포넌트를 물고있는 게임오브젝트를 찾을 것이다.
        Bind<Text>(typeof(Texts));

        Bind<GameObject>(typeof(GameObjects));

        //T Get<T>에서 T가 리턴하면서 Text가 튀어나옴, 2021-07-16
        //Get<Text>((int)Texts.ScoreText).text = "Bind Test";

        Bind<Image>(typeof(Images));


        //하이어라키 창에 있는 ItemIcon 게임오브젝트를 가져온다.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;

        //2021-07-17
        BindEvent(go, (PointerEventData data) => { go.gameObject.transform.position = data.position; }, Define.UIEvent.Drag);

        //버튼 클릭시 점수 상승. Extension 메소드를 사용하여 만들었음(Extension.cs) ,2021-07-17 
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
    }


    int score = 0;

    //꼭 public으로 해준다. UI로 쓰려면.
    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("ButtonClicked");

        score++;

        //버튼 누르면 점수 상승하는 것을 출력, 2021-07-17
        GetText((int)Texts.ScoreText).text = $"점수 : {score}";
    }
}